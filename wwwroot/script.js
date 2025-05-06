function openPopup() {
  document.getElementById("loginPopup").style.display = 'flex';
}

function closePopup() {
  document.getElementById("loginPopup").style.display = 'none';
}

document.getElementById("loginPopup").addEventListener('submit', async function (e) {
  e.preventDefault();
  const email = e.target.email.value
  const senha = e.target.senha.value
  
  const resposta = await fetch('http://localhost:5000/api/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, senha })
  });

  const resultado = await resposta.json();

  if (resposta.ok) {
      alert("Login OK!");
      // Redirecionar ou carregar dados do usuário
  } else {
      alert(resultado.mensagem);
  }
})