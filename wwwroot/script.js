let logado = false;

function verificaLogin() {
  if (!logado) {
    alert("Você precisa estar logado para interagir com as publicações.");
  }
}

function exibirPopupLogin() {
  // Aqui poderia abrir um modal real
  const usuario = prompt("Digite seu nome de usuário:");
  if (usuario) {
    logado = true;
    alert("Bem-vindo, " + usuario + "!");
  }
}
