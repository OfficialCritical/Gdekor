document.addEventListener('DOMContentLoaded',function(){

    // CSAK EGÉSZ SZÁN
    document.addEventListener('keydown',function(e){
        if(e.target.classList.contains('inpt_egeszSzam'))
        {
            if([',','.','+','-','e','E'].includes(e.key))
            {
                e.preventDefault();
            }
        }
    });

    document.addEventListener('input', function (e) {
        if(e.target.classList.contains('inpt_egeszSzam'))
        {
            e.target.value=e.target.value.replace(/\D/g,'');
        }
    });



    // 10000 --> 10 000
    document.addEventListener('input',function(e){
        if(e.target.classList.contains('inpt_Penz'))
        {
            let ertek = e.target.value.replace(/[^\d-]/g, '')
            e.target.value = ertek ? Number(ertek).toLocaleString('hu-HU') : '';
        }
    });


});