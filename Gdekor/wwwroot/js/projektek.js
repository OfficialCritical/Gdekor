document.addEventListener('DOMContentLoaded', function () {

    const animatedDiv = document.querySelector('.animatedDiv');
    const cardMezok = document.querySelector('.cardMezok');
    const form_Projekt = document.getElementById('form_Projekt');

    const bttn_uj = document.querySelector('.bttn_uj');
    const p_Valaszto = document.getElementById('p_Valaszto');
    const p_reszletek = document.getElementById('p_reszletek');
    

    
    const szTipus_select = document.getElementById('szTipus_select');
    const szHozza_Div = document.querySelector('.szHozza_Div');
    const userLista_Div = document.querySelector('.userLista_Div');
    const bttn_szList_Megtek = document.getElementById('bttn_szList_Megtek');
    const emHozza_select = document.getElementById('emHozza_select');
    
    if (!p_Valaszto || !p_reszletek) return;


    animatedDiv.classList.add('elohiv');

    const vanHiba = document.querySelector('.validation-summary-errors')|| document.querySelector('span.field-validation-error');
    if (vanHiba) {
        
        document.querySelectorAll('.cim_Div').forEach(function (cim) {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            reszletek.classList.add('elohiv');
            nyil.classList.add('srehen');
        });
        p_reszletek.classList.add('aktiv');

        const proId = document.getElementById('Pro_ID_Edit')?.value;
        if (proId) p_Valaszto.value = proId;
    }

    function mezoBeallit(id, ertek) {
        const inp = document.getElementById(id);

        if (!inp) return;

        const value = ertek?.trim() ?? '';
        const fp = window.datumFp?.[id];

        if (fp && typeof fp.setDate === 'function') {
            if (value) {
                fp.setDate(value, false, 'Y-m-d');
            }
            else {
                fp.clear();
            }
        }
        else {
            inp.value = value;
        }
    }

    p_Valaszto.addEventListener('change', function () {

        const opt = p_Valaszto.selectedOptions[0];

        if (p_Valaszto.value) {

            bttn_uj.classList.remove('aktiv');
            
            p_reszletek.classList.add('aktiv');
            document.querySelectorAll('.cim_Div').forEach(function (cim) {
                const reszletek = cim.nextElementSibling;
                const nyil = cim.querySelector('.forgoNyil');

                reszletek.classList.add('elohiv');
                nyil.classList.add('srehen');            
            });
            const d = opt.dataset;
            console.table({
                tervKezd: d.tervKezd,
                tervVeg: d.tervVeg,
                valosKezd: d.valosKezd,
                valosVeg: d.valosVeg,
                bevetel: d.bevetel,
                koltseg: d.koltseg,
                profit: d.profit
            });
            document.getElementById('Pro_ID_Edit').value = p_Valaszto.value;
            document.getElementById('Nev_Edit').value = d.nev ?? '';
            document.getElementById('Allapot_Edit').value = d.allapot ?? '';
            document.getElementById('Leir_Edit').value = d.leir ?? '';
            document.getElementById('szTipus_select').value = d.kik ?? 'mindenki';

            document.getElementById('Bevetel_Edit').value = d.bevetel ?? '';
            document.getElementById('Koltseg_Edit').value = d.koltseg ?? '';
            document.getElementById('Profit_Edit').value = d.profit ?? '';

            mezoBeallit('TervKezd_Edit', d.tervKezd);
            mezoBeallit('TervVeg_Edit', d.tervVeg);
            mezoBeallit('ValosKezd_Edit', d.valosKezd);
            mezoBeallit('ValosVeg_Edit', d.valosVeg);


        }
        else {
            form_Projekt.reset();
            document.getElementById('Pro_ID_Edit').value = '';
            p_reszletek.classList.remove('aktiv');
        }
    });

    bttn_uj.addEventListener('click', function () {
        form_Projekt.reset();
        document.getElementById('Pro_ID_Edit').value = '';
        bttn_uj.classList.add('aktiv');        
        p_reszletek.classList.add('aktiv');        
        document.querySelectorAll('.cim_Div').forEach(function (cim) {  
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            reszletek.classList.add('elohiv');
            nyil.classList.add('srehen');            
        });
    });

    bttn_szList_Megtek.addEventListener('click', function () {

        bttn_szList_Megtek.classList.add('elrejt');
        emHozza_select.classList.add('elohiv');
        szHozza_Div.classList.add('kiemel');
        userLista_Div.classList.add('elohiv');
    });
    /*
    szTipus_select.addEventListener('change', function () {
        if (szTipus_select.value === 'mindenki') {

        }
        else if (szTipus_select.value === 'egyeni') {

        }
    });
    */






    document.querySelectorAll('.cim_Div').forEach(function (cim) {
        cim.addEventListener('click', function () {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            if (!reszletek || !reszletek.classList.contains('reszletek_Div')) return;

            if (reszletek.classList.contains('elohiv')) {
                reszletek.classList.remove('elohiv');
                nyil.classList.remove('srehen');
            }
            else {
                reszletek.classList.add('elohiv');
                nyil.classList.add('srehen');
            }
        });
    });

})
