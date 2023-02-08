var formulario = document.getElementById('Formulario');

formulario.addEventListener('submit', function(e){
    e.preventDefault();
    console.log('Me diste un click')

    var datos = new FormData(formulario)   

    fetch('https//localhost:7292/api/Cuentas/Login', {
        method: 'POST',
        body: datos
    })
        .then(res => res.json())
        .then(data => {
            console.log(data)
        })
    
});