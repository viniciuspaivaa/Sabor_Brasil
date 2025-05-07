function openPopup() {
  document.getElementById("loginPopup").style.display = 'flex';
}

function closePopup() {
  document.getElementById("loginPopup").style.display = 'none';
}

document.getElementById("loginPopup").addEventListener('submit  ', async function (e) {
  e.preventDefault();
  console.log("Form enviado!"); 
  const email = e.target.email.value
  const senha = e.target.senha.value
  
  const resposta = await fetch('http://localhost:5000/api/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, senha })
  });

  const resultado = await resposta.json();
  console.log(resultado)

  if (resposta.ok) {
      alert("Login OK!");
      document.getElementById("perfilNome").textContent = `${resultado.nome}!`;
  } else {
      alert(resultado.mensagem);
  }
})