
window.addEventListener("load", function () {
    
    try {
        var texto = document.getElementsByClassName("collapse-lg")
        var collapse = 0

        const myObserver = new ResizeObserver(entries => {
            if (entries[0].contentRect.width > 994 && collapse != 1) {
                for (var i = 0; i < texto.length; i++) {
                    texto[i].classList.add("show")
                }
                collapse = 1
            } else if (entries[0].contentRect.width < 994 && collapse != -1) {
                for (var i = 0; i < texto.length; i++) {
                    texto[i].classList.remove("show")
                }
                collapse = -1
            }   
        });

        myObserver.observe(document.body)
        
    }catch(e){
        console.log(e)
    }
    
})
