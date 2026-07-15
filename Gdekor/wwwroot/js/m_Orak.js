document.addEventListener('DOMContentLoaded', function () {

    const animatedDiv = document.querySelector('.animatedDiv');
    const form_mOrak = document.getElementById('form_mOrak');

    const m_kezdete = document.getElementById('m_kezdete');
    const m_vege = document.getElementById('m_vege');
    
    
    const bttn_mNapMentes = document.getElementById('bttn_mNapMentes');
    
    if (!m_kezdete || !m_vege || !animatedDiv) {
        return;
    }
    
    animatedDiv.classList.add('elohiv');



    /*
    const sz_kezdete = document.getElementById('sz_kezdete');
    const sz_vege = document.getElementById('sz_vege');
    
    const btt_szHozzaad = document.getElementById('btt_szHozzaad');
    const szunetekLista = document.getElementById('szunetekLista');
    const noSzunet = document.getElementById('noSzunet');

        form_mOrak?.addEventListener('submit', function (e) {
        const datum = document.getElementById('Datum_Edit')?.value;
        const projekt = document.getElementById('select_Projekt')?.value;
        if (!datum) {
            e.preventDefault();
            alert('Válassz ki egy dátumot a naptárban!');
            return;
        }
        if (!projekt) {
            e.preventDefault();
            alert('Válassz projektet!');
            return;
        }
        const szunetek = [];
        szunetekLista.querySelectorAll('li').forEach(li => {
            szunetek.push({
                Kezdete: li.dataset.kezdete,
                Vege: li.dataset.vege
            });
        });
        document.getElementById('Szunetek_Json').value = JSON.stringify(szunetek);
    });

    function frissitUresSzoveg() {
        noSzunet.style.display = szunetekLista.children.length ? 'none' : 'block';
    }

    function sz_atfedes(kezdet1, veg1, kezdet2, veg2) {
        return kezdet1 < veg2 && veg1 > kezdet2;
    }

    function vanSzunetAtfedes(ujKezdet, ujveg) {
        const sorok = szunetekLista.querySelectorAll('li');

        for (const sor of sorok) {
            const regiKezdet = sor.dataset.kezdete;
            const regiVeg = sor.dataset.vege;

            if (sz_atfedes(ujKezdet, ujveg, regiKezdet, regiVeg)) {
                return true;
            }
        }
        return false;
    }

    btt_szHozzaad.addEventListener('click', function () {
        const kezdete = sz_kezdete.value;
        const vege = sz_vege.value;

        if (!kezdete || !vege) {
            alert('Kérlek add meg a szünet kedetét és végét!');
            return;
        }

        if (kezdete >= vege) {
            alert('A szünet vége későbbi időpont kell legyen, mint a kezdete!');
            return;
        }

        if (vanSzunetAtfedes(kezdete, vege)) {
            alert('Szünetek között nem lehet átfedés!');
            return;
        }

        const li = document.createElement('li');
        li.className = 'list-group-item';
        li.dataset.kezdete = kezdete;
        li.dataset.vege = vege;

        li.innerHTML = `
            <span > ${kezdete} - ${vege}</span >
            <button type="button" class="btn btn-sm btn-outline-danger bttnSz_Torol">törlés</button>
        `;

        li.querySelector('.bttnSz_Torol').addEventListener('click', function () {
            li.remove();
            frissitUresSzoveg();
        });

        szunetekLista.appendChild(li);

        sz_kezdete.value = '';
        sz_vege.value = '';
        frissitUresSzoveg();
    });

    
    frissitUresSzoveg();
    */ 


});