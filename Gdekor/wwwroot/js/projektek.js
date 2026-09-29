document.addEventListener('DOMContentLoaded', function () {

    const animatedDiv = document.querySelector('.animatedDiv');
    const form_Projekt = document.getElementById('form_Projekt');

    const bttn_uj = document.querySelector('.bttn_uj');
    const p_Valaszto = document.getElementById('p_Valaszto');
    const p_reszletek = document.getElementById('p_reszletek');

    //const szTipus_select = document.getElementById('szTipus_select');
    const emHozza_select = document.getElementById('emHozza_select');
    const userLista_Tbl_Bdy = document.querySelector('.userLista_Tbl tbody');

    if (!p_Valaszto || !p_reszletek) return;

    if (animatedDiv) {
        animatedDiv.classList.add('elohiv');
    }

    const vanHiba = document.querySelector('.validation-summary-errors')
        || document.querySelector('span.field-validation-error');

    if (vanHiba) {
        document.querySelectorAll('.cim_Div').forEach(function (cim) {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            reszletek.classList.add('elohiv');
            nyil.classList.add('srehen');
        });
        p_reszletek.classList.add('aktiv');

        const proId = document.getElementById('Pro_ID_Edit')?.value;
        if (proId) {
            p_Valaszto.value = proId;
        }
    }

    function mezoBeallit(id, ertek) {
        const inp = document.getElementById(id);
        if (!inp) return;

        const value = ertek?.trim() ?? '';
        const fp = window.datumFp?.[id];

        if (fp && typeof fp.setDate === 'function') {
            if (value) {
                fp.setDate(value, false, 'Y-m-d');
            } else {
                fp.clear();
            }
        } else {
            inp.value = value;
        }
    }

    async function betoltResztvevok(proId) {
        userLista_Tbl_Bdy.innerHTML = '';
        const response = await fetch(`?handler=Resztvevok&proId=${encodeURIComponent(proId)}`);

        if (!response.ok) {
            alert('Hiba lépet fel!');
            return;
        }

        const lista = await response.json();

        if (p_Valaszto.value !== proId) return;

        try {
            lista.forEach(emberke => {
                const tr = document.createElement('tr');
                tr.dataset.id = emberke.id || '';
                tr.dataset.userId = emberke.userId || '';
                tr.innerHTML = `
                <td>
                    <button type="button" class="btn btn-outline-danger bttn_userTorol"> törlés </button>
                </td>
                <td class="userNev">                    
                </td>
                <td>
                    <input type="number" min="0" class="form-control userOraber" placeholder="Ft">
                </td>
                <td>
                    <input type="number" min="0" class="form-control userNapiber_Ft" placeholder="Ft">
                </td>
                <td>
                    <input type="number" min="0" class="form-control userNapiber_Ora" placeholder="max hány óra">
                </td>
            `;
                tr.querySelector('.userNev').textContent = emberke.nev ?? '';
                tr.querySelector('.userOraber').value = emberke.oraber ?? '';
                tr.querySelector('.userNapiber_Ft').value = emberke.napiber_Ft ?? '';
                tr.querySelector('.userNapiber_Ora').value = emberke.napiber_Ora ?? '';

                userLista_Tbl_Bdy.appendChild(tr);
        })
        }
        catch (error) {
            alert('Hiba történt a résztvevő személyek adatainak betöltésekor!');
        }

    }

    p_Valaszto.addEventListener('change', function () {
        const opt = p_Valaszto.selectedOptions[0];

        if (p_Valaszto.value) {
            bttn_uj?.classList.remove('aktiv');

            p_reszletek.classList.add('aktiv');
            document.querySelectorAll('.cim_Div').forEach(function (cim) {
                const reszletek = cim.nextElementSibling;
                const nyil = cim.querySelector('.forgoNyil');

                reszletek.classList.remove('elohiv');
                nyil.classList.remove('srehen');
            });

            const d = opt.dataset;

            document.getElementById('Pro_ID_Edit').value = p_Valaszto.value;
            document.getElementById('Nev_Edit').value = d.nev ?? '';
            document.getElementById('Allapot_Edit').value = d.allapot ?? '';
            document.getElementById('Leir_Edit').value = d.leir ?? '';

            document.getElementById('Bevetel_Edit').value = d.bevetel ?? '';
            document.getElementById('Koltseg_Edit').value = d.koltseg ?? '';
            document.getElementById('Profit_Edit').value = d.profit ?? '';

            mezoBeallit('TervKezd_Edit', d.tervKezd);
            mezoBeallit('TervVeg_Edit', d.tervVeg);
            mezoBeallit('ValosKezd_Edit', d.valosKezd);
            mezoBeallit('ValosVeg_Edit', d.valosVeg);

            betoltResztvevok(p_Valaszto.value);

        } else {
            form_Projekt.reset();
            document.getElementById('Pro_ID_Edit').value = '';
            userLista_Tbl_Bdy.innerHTML = '';
            p_reszletek.classList.remove('aktiv');
        }
    });

    bttn_uj?.addEventListener('click', function () {
        form_Projekt.reset();
        userLista_Tbl_Bdy.innerHTML = '';
        document.getElementById('Pro_ID_Edit').value = '';
        p_Valaszto.value = '';
        bttn_uj.classList.add('aktiv');
        p_reszletek.classList.add('aktiv');

        document.querySelectorAll('.cim_Div').forEach(function (cim) {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            reszletek.classList.add('elohiv');
            nyil.classList.add('srehen');
        });


    });


    emHozza_select.addEventListener('change', function () {
        if (emHozza_select.value === '') return;

        const userID = emHozza_select.value;
        const userNev = emHozza_select.selectedOptions[0].textContent.trim();

        const letezik = [...userLista_Tbl_Bdy.querySelectorAll('tr')].some(tr => tr.dataset.userId === userID);
        if (letezik) {
            emHozza_select.value = '';
            alert('A kiválasztott személy már szerepel a lentebbi listában!');
            return;
        }

        const tr = document.createElement('tr');
        tr.dataset.userId = userID;
        tr.dataset.id = '';
        tr.innerHTML = `
            <td>
                <button type="button" class="btn btn-outline-danger bttn_userTorol"> törlés </button>
            </td>
            <td class="userNev">
            </td>
            <td>
                <input type="number" class="form-control userOraber" min="0" placeholder="Ft">
            </td>
            <td>
                <input type="number" class="form-control userNapiber_Ft" min="0" placeholder="Ft">
            </td>
            <td>
                <input type="number" class="form-control userNapiber_Ora" min="0" placeholder="max hány óra">
            </td>
        `;
        tr.querySelector('.userNev').textContent = userNev;
        userLista_Tbl_Bdy.appendChild(tr);
        emHozza_select.value = '';
    });
    
    userLista_Tbl_Bdy.addEventListener('click', function (e) {
        if (e.target.classList.contains('bttn_userTorol')) {
            e.target.closest('tr').remove();
        }
    })
    
    form_Projekt.addEventListener('submit', function () {
        const resztvevok = [];

        userLista_Tbl_Bdy.querySelectorAll('tr').forEach(tr => {
            resztvevok.push({
                Id: tr.dataset.id || null,
                UserId: tr.dataset.userId || null,
                Nev: tr.querySelector('.userNev').textContent.trim(),
                Oraber: tr.querySelector('.userOraber').value,
                Napiber_Ft: tr.querySelector('.userNapiber_Ft').value,
                Napiber_Ora: tr.querySelector('.userNapiber_Ora').value
            });
        });
        document.getElementById('Resztvevok_Json').value = JSON.stringify(resztvevok);
    });

    document.querySelectorAll('.cim_Div').forEach(function (cim) {
        cim.addEventListener('click', function () {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            if (!reszletek || !reszletek.classList.contains('reszletek_Div')) return;

            if (reszletek.classList.contains('elohiv')) {
                reszletek.classList.remove('elohiv');
                nyil.classList.remove('srehen');
            } else {
                reszletek.classList.add('elohiv');
                nyil.classList.add('srehen');
            }
        });
    });

});