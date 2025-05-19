function openPopup() {
  document.getElementById("loginPopup").style.display = 'flex';
}

function closePopup() {
  document.getElementById("loginPopup").style.display = 'none';
}



async function carregarPosts(usuarioId) {
  const resposta = await fetch(`http://localhost:5000/api/post/${usuarioId}`); // rota correta!
  
  if (!resposta.ok) {
    console.error(`Erro ao carregar posts (status: ${resposta.status})`);
    return;
  }

  const posts = await resposta.json();

  const container = document.getElementById("postsContainer");
  container.innerHTML = ""; // limpa os posts anteriores

  posts.forEach( async post => {
    const postEl = document.createElement("div");
    postEl.classList.add("post");

    const data = new Date(post.criadoEm)
    const dataFormatada = data.toLocaleDateString("pt-BR", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric"
    })

    postEl.innerHTML = `
      <img src="${post.imgPrato}" alt="${post.titulo}" class="prato">
      <h3>${post.titulo}</h3>
      <p class="descricao">Local: ${post.descricao} - Cidade: ${post.cidade} - Publicado em: ${dataFormatada}</p>
      <div class="reacoes" data-id="${post.id}">
        <button class="like">👍 <span class="like-count">0</span></button>
        <button class="deslike">👎 <span class="deslike-count">0</span></button>
        <button class="mensagens" data-post="${post.id}">💬 Mensagens</button>
      </div>
    `;

    container.appendChild(postEl);
    await atualizarReacoes(post.id);
  });
}

async function atualizarReacoes(idPostagem) {
  const resposta = await fetch(`http://localhost:5000/api/reacao/${idPostagem}`);
  const { likes, deslikes } = await resposta.json();

  const reacoes = document.querySelector(`.reacoes[data-id='${idPostagem}']`);
  if (reacoes) {
    reacoes.querySelector(".like-count").textContent = likes;
    reacoes.querySelector(".deslike-count").textContent = deslikes;
  }
}

async function atualizarReacoesPerfil(usuarioId) {
  // Busca todos os posts do usuário
  const resposta = await fetch(`http://localhost:5000/api/post/${usuarioId}`);
  if (!resposta.ok) return;

  const posts = await resposta.json();
  let totalLikes = 0;
  let totalDeslikes = 0;

  // Para cada post, busca as reações
  for (const post of posts) {
    const respReacao = await fetch(`http://localhost:5000/api/reacao/${post.id}`);
    if (!respReacao.ok) continue;
    const { likes, deslikes } = await respReacao.json();
    totalLikes += likes;
    totalDeslikes += deslikes;
  }

  // Atualiza o HTML
  document.getElementById("perfilReacoes").innerHTML = `
    <span>👍 Likes: ${totalLikes} &nbsp;&nbsp; 👎 Deslikes: ${totalDeslikes}</span>
  `;
}


document.addEventListener('DOMContentLoaded', () => {
  const form = document.getElementById("loginPopup");
  
  form.addEventListener("submit", async function (e) {
    e.preventDefault(); // Impede o envio tradicional

    const email = e.target.email.value;
    const senha = e.target.senha.value;

    const resposta = await fetch('http://localhost:5000/api/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ email, senha })
    });

    const dados = await resposta.json();

    if (resposta.ok) {
      alert("Login OK");
      document.getElementById("perfilNome").textContent = dados.nome;
      
      // Adicione esta linha:
      window.usuarioIdLogado = dados.id;

      closePopup();

      document.getElementById("logout").style.display = 'flex';
      document.getElementById("login").style.display = 'none';

      const img = document.getElementById("fotoPerfil");
      img.src = `http://localhost:5000/${dados.imagem}`;

      carregarPosts(dados.id);
      atualizarReacoesPerfil(dados.id);
    } else {
      alert(dados.mensagem || "Erro no login");
    }
  });
});

let postIdAtual = null;
let usuarioIdAtual = null;

document.addEventListener('click', async function(e) {
  if (e.target.classList.contains('mensagens')) {
    postIdAtual = e.target.getAttribute('data-post');
    // Supondo que você já tem o id do usuário logado em uma variável global
    usuarioIdAtual = window.usuarioIdLogado || null;

    const postId = e.target.getAttribute('data-post');
    const resp = await fetch(`http://localhost:5000/api/comentario/${postId}`);
    const comentarios = await resp.json();

    const lista = document.getElementById('comentariosLista');
    lista.innerHTML = '';

    if (comentarios.length === 0) {
      lista.innerHTML = '<p>Nenhum comentário ainda.</p>';
    } else {
      comentarios.forEach(c => {
        lista.innerHTML += `
          <div style="border-bottom:1px solid #eee; margin-bottom:10px; padding-bottom:8px;">
            <strong>${c.nomeUsuario || 'Usuário'}</strong> 
            <span style="font-size:12px;color:#888;">${new Date(c.data).toLocaleDateString('pt-BR')}</span>
            <p>${c.texto}</p>
            ${c.imagem ? `<img src="http://localhost:5000/${c.imagem}" style="max-width:80px;max-height:80px;">` : ''}
          </div>
        `;
      });
    }

    document.getElementById('comentariosModal').style.display = 'flex';
  }
});

document.getElementById('fecharModal').onclick = function() {
  document.getElementById('comentariosModal').style.display = 'none';
};

document.getElementById('formComentario').onsubmit = async function(e) {
  e.preventDefault();
  if (!postIdAtual || !usuarioIdAtual) {
    alert("Usuário não identificado!");
    return;
  }
  const texto = document.getElementById('textoComentario').value;
  const imagem = document.getElementById('imagemComentario').files[0];

  const formData = new FormData();
  formData.append('idPostagem', postIdAtual);
  formData.append('idUsuario', usuarioIdAtual);
  formData.append('texto', texto);
  if (imagem) formData.append('imagem', imagem);

  const resp = await fetch('http://localhost:5000/api/comentario', {
    method: 'POST',
    body: formData
  });

  if (resp.ok) {
    document.getElementById('textoComentario').value = '';
    document.getElementById('imagemComentario').value = '';
    // Recarrega comentários
    const comentariosResp = await fetch(`http://localhost:5000/api/comentario/${postIdAtual}`);
    const comentarios = await comentariosResp.json();
    const lista = document.getElementById('comentariosLista');
    lista.innerHTML = '';
    if (comentarios.length === 0) {
      lista.innerHTML = '<p>Nenhum comentário ainda.</p>';
    } else {
      comentarios.forEach(c => {
        lista.innerHTML += `
          <div style="border-bottom:1px solid #eee; margin-bottom:10px; padding-bottom:8px;">
            <strong>${c.nomeUsuario || 'Usuário'}</strong> 
            <span style="font-size:12px;color:#888;">${new Date(c.data).toLocaleDateString('pt-BR')}</span>
            <p>${c.texto}</p>
            ${c.imagem ? `<img src="http://localhost:5000/comentarios/${c.imagem}" style="max-width:80px;max-height:80px;">` : ''}
          </div>
        `;
      });
    }
  } else {
    alert("Erro ao enviar comentário!");
  }
};
