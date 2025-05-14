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

  posts.forEach(post => {
    const postEl = document.createElement("div");
    postEl.classList.add("post");

    postEl.innerHTML = `
      <img src="${post.imgPrato}" alt="${post.titulo}" class="prato">
      <h3>${post.titulo}</h3>
      <p class="descricao">Local: ${post.descricao} - Cidade: ${post.cidade}</p>
      <div class="reacoes">
        <!--Inserir likes e deslikes futuramente-->
      </div>
    `;

    container.appendChild(postEl);
  });
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
      
      closePopup()

      document.getElementById("logout").style.display = 'flex';
      document.getElementById("login").style.display = 'none';

      const img = document.getElementById("fotoPerfil");
      img.src = `http://localhost:5000/${dados.imagem}`;

      carregarPosts(dados.id);
    } else {
      alert(dados.mensagem || "Erro no login");
    }

    
  });
});
