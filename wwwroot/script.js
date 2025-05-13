function openPopup() {
  document.getElementById("loginPopup").style.display = 'flex';
}

function closePopup() {
  document.getElementById("loginPopup").style.display = 'none';
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
    console.log(dados.imagem)


    if (resposta.ok) {
      alert("Login OK");
      document.getElementById("perfilNome").textContent = dados.nome;
      
      closePopup()

      document.getElementById("logout").style.display = 'flex';
      document.getElementById("login").style.display = 'none';

      const img = document.getElementById("fotoPerfil");
      img.src = `http://localhost:5000/${dados.imagem}`;

    } else {
      alert(dados.mensagem || "Erro no login");
    }
  });
});
