document.addEventListener('DOMContentLoaded', function () {

    const animatedDiv = document.querySelector('.animatedDiv');
    const form_Projekt = document.getElementById('form_Projekt');

    const bttn_uj = document.querySelector('.bttn_uj');
    const p_Valaszto = document.getElementById('p_Valaszto');
    const p_reszletek = document.getElementById('p_reszletek');

    //const szTipus_select = document.getElementById('szTipus_select');
    const emHozza_select = document.getElementById('emHozza_select');    
    const userLista_Tbl_Head = document.querySelector('.userLista_Tbl thead');
    const oraszam_Tbl_Head = document.querySelector('.oraszam_Tbl thead');
    const userLista_Tbl_Bdy = document.querySelector('.userLista_Tbl tbody');
    const oraszam_Tbl_Bdy = document.querySelector('.oraszam_Tbl tbody');

    const ugyesMegtekint_lbl = document.getElementById('ugyesMegtekint_lbl');
    const ugyesElrejt_lbl = document.getElementById('ugyesElrejt_lbl');
    const ugyeskedok_Tartalom = document.querySelector('.ugyeskedok_Tartalom');
    const ugyeskedok_Adatok = document.getElementById('ugyeskedok_Adatok');

    const Bevetel_Edit= document.getElementById('Bevetel_Edit');
    const Koltseg_Edit= document.getElementById('Koltseg_Edit');
    const inpt_berKoltseg= document.getElementById('inpt_berKoltseg');
    const Egyeb_reszKolt_Edit= document.getElementById('Egyeb_reszKolt_Edit');
    const Profit_Edit= document.getElementById('Profit_Edit');

    if (!animatedDiv || !form_Projekt || !bttn_uj || !p_Valaszto || !p_reszletek || !emHozza_select || !userLista_Tbl_Head || !oraszam_Tbl_Head || !userLista_Tbl_Bdy || !oraszam_Tbl_Bdy) 
        return;

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

    document.addEventListener('keydown',function(e){
        if (e.target.classList.contains('userOraber') || e.target.classList.contains('userNapiber_Ft') || e.target.classList.contains('userNapiber_Ft')) 
        {
            if(['.',',','-','+','e','E'].includes(e.key))
                e.preventDefault();
        }
    });


    function penzFormaz(ertek){

        const szam = String(ertek ?? '').replace(/\D/g,'');
        return szam ? Number(szam).toLocaleString('hu-HU') : '';

    };


    async function betoltResztvevok(proId) {

        userLista_Tbl_Bdy.innerHTML = '';
        oraszam_Tbl_Bdy.innerHTML = '';

        ugyeskedok_Adatok.innerHTML = '';

        const response = await fetch(`?handler=Resztvevok&proId=${encodeURIComponent(proId)}`);

        if (!response.ok) {
            alert('Hiba lépet fel!');
            return;
        }

        const adat = await response.json();
        const lista = adat.resztvevok;
        const oraszamok = adat.oraszamok;
        const ugyeskedok = adat.ugyeskedok;
        const osszesKoltseg=adat.osszesKoltseg;

        inpt_berKoltseg.value = penzFormaz(osszesKoltseg);

        if (p_Valaszto.value !== proId) return;

        try {

            // ----------------------------------------
            // RÉSZTVEVŐK / BÉRADATOK
            // ----------------------------------------

            if (lista.length === 0) {

                userLista_Tbl_Head.style.display = 'none';

                const tr = document.createElement('tr');
                tr.classList.add('uresSor');

                tr.innerHTML = `
                    <td>
                        Nincsenek elmentett béradatok.
                    </td>
                `;

                userLista_Tbl_Bdy.appendChild(tr);
            }
            else {

                userLista_Tbl_Head.style.display = '';

                lista.forEach(emberke => {

                    const tr = document.createElement('tr');

                    tr.dataset.id = emberke.id || '';
                    tr.dataset.userId = emberke.userId || '';

                    tr.innerHTML = `
                        <td>
                            <button type="button"
                                    class="btn btn-outline-danger bttn_userTorol">
                                törlés
                            </button>
                        </td>

                        <td class="userNev"></td>

                        <td>
                            <input type="number"
                                   min="0"
                                   class="form-control userOraber inpt_egeszSzam inpt_Penz"
                                   placeholder="Ft">
                        </td>

                        <td>
                            <input type="number"
                                   min="0"
                                   class="form-control userNapiber_Ft inpt_egeszSzam inpt_Penz"
                                   placeholder="Ft">
                        </td>

                        <td>
                            <input type="number"
                                   min="0"
                                   class="form-control userNapiber_Ora inpt_egeszSzam"
                                   placeholder="max hány óra">
                        </td>
                    `;

                    tr.querySelector('.userNev').textContent =
                        emberke.nev ?? '';

                    tr.querySelector('.userOraber').value =
                        emberke.oraber ?? '';

                    tr.querySelector('.userNapiber_Ft').value =
                        emberke.napiber_Ft ?? '';

                    tr.querySelector('.userNapiber_Ora').value =
                        emberke.napiber_Ora ?? '';

                    userLista_Tbl_Bdy.appendChild(tr);
                });


                // ----------------------------------------
                // ÓRASZÁMOK
                // ----------------------------------------

                oraszam_Tbl_Head.style.display = '';

                oraszamok.forEach(ember => {

                    const tr = document.createElement('tr');

                    tr.innerHTML = `
                        <td class="text-center">
                            ${ember.nev ?? ''}
                        </td>

                        <td class="text-center">
                            ${ember.osszOra ?? ''}
                        </td>

                        <td class="text-center">
                            ${ember.tulora ?? ''}
                        </td>

                        <td class="text-center inpt_Penz">
                            ${ember.fizetes ?? '?'}
                        </td>
                    `;

                    oraszam_Tbl_Bdy.appendChild(tr);
                });
            }


            // ----------------------------------------
            // ÜGYESKEDŐK
            // ----------------------------------------

            if (!ugyeskedok || ugyeskedok.length === 0) {

                ugyeskedok_Adatok.innerHTML = `
                    <div class="ms-2 mb-3">
                        Nincs többszörösen felvitt munkaidő.
                    </div>
                `;
            }
            else {

                ugyeskedok.forEach(user => {

                    // Egy ember teljes blokkja
                    const userDiv = document.createElement('div');
                    userDiv.classList.add('mb-4','d-flex','gap-3');


                    // Név
                    const nevDiv = document.createElement('div');

                    nevDiv.classList.add('mb-2', 'fst-italic');
                    nevDiv.style.color='orange';
                    nevDiv.style.whiteSpace='nowrap';
                    nevDiv.textContent = user.nev ?? 'Ismeretlen';

                    userDiv.appendChild(nevDiv);


                    // Az ember problémás napjai
                    user.napok.forEach(nap => {

                        const table = document.createElement('table');

                        table.classList.add(
                            'table',
                            'ugyeskedok_Tbl',
                            'mb-3'
                        );

                        const darab = nap.adatok.length;

                        let sorszamSor = '';
                        let kezdetSor = '';
                        let vegSor = '';
                        let oraszamSor  = '';


                        nap.adatok.forEach((munka, index) => {

                            sorszamSor += `
                                <td class="text-center">
                                    ${index + 1}.
                                </td>
                            `;

                            kezdetSor += `
                                <td class="text-center">
                                    ${munka.kezdet ?? ''}
                                </td>
                            `;

                            vegSor += `
                                <td class="text-center">
                                    ${munka.veg ?? ''}
                                </td>
                            `;

                            let oraszam = '';

                            if (munka.kezdet && munka.veg) {
                                const [kOra, kPerc] = munka.kezdet.split(':').map(Number);
                                const [vOra, vPerc] = munka.veg.split(':').map(Number);

                                oraszam = ((vOra * 60 + vPerc) - (kOra * 60 + kPerc)) / 60;
                            }

                            oraszamSor += `
                                <td class="text-center">
                                    ${oraszam}
                                </td>
                            `;
                        });


                        table.innerHTML = `

                            <thead>
                                <tr>
                                    <th></th>

                                    <th colspan="${darab}"
                                        class="text-center">

                                        ${nap.nap ?? ''}

                                    </th>
                                </tr>
                            </thead>

                            <tbody>

                                <tr>
                                    <th>
                                        Felvitel
                                    </th>

                                    ${sorszamSor}
                                </tr>

                                <tr>
                                    <th>
                                        Kezdet
                                    </th>

                                    ${kezdetSor}
                                </tr>

                                <tr>
                                    <th>
                                        Vég
                                    </th>

                                    ${vegSor}
                                </tr>

                                <tr>
                                    <th>
                                        Óraszám
                                    </th>

                                    ${oraszamSor}
                                </tr>
                            </tbody>
                        `;

                        userDiv.appendChild(table);
                    });


                    ugyeskedok_Adatok.appendChild(userDiv);
                });
            }

        }
        catch (error) {

            console.error(error);

            alert(
                'Hiba történt a résztvevő személyek adatainak betöltésekor!'
            );
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
            document.getElementById('Egyeb_reszKolt_Edit').value = d.egyebkoltseg ?? '';
            document.getElementById('Profit_Edit').value = d.profit ?? '';

            Bevetel_Edit.value = penzFormaz(d.bevetel);
            Koltseg_Edit.value = penzFormaz(d.koltseg);
            Egyeb_reszKolt_Edit.value = penzFormaz(d.egyebkoltseg);
            Profit_Edit.value = penzFormaz(d.profit);

            mezoBeallit('TervKezd_Edit', d.tervKezd);
            mezoBeallit('TervVeg_Edit', d.tervVeg);
            mezoBeallit('ValosKezd_Edit', d.valosKezd);
            mezoBeallit('ValosVeg_Edit', d.valosVeg);

            betoltResztvevok(p_Valaszto.value);

        } else {
            form_Projekt.reset();
            document.getElementById('Pro_ID_Edit').value = '';
            p_reszletek.classList.remove('aktiv');
        }
    });

    bttn_uj?.addEventListener('click', function () {
        form_Projekt.reset();        
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

        userLista_Tbl_Head.style.display='';

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
                <input type="number" class="form-control userOraber" step="1"  min="0" placeholder="Ft">
            </td>
            <td>
                <input type="number" class="form-control userNapiber_Ft" step="1" min="0" placeholder="Ft">
            </td>
            <td>
                <input type="number" class="form-control userNapiber_Ora" step="1" min="0" placeholder="max hány óra">
            </td>
        `;
        tr.querySelector('.userNev').textContent = userNev;
        userLista_Tbl_Bdy.appendChild(tr);
        emHozza_select.value = '';
        userLista_Tbl_Bdy.querySelector('.uresSor')?.remove();
    });
    
    userLista_Tbl_Bdy.addEventListener('click', function (e) {
        if (e.target.classList.contains('bttn_userTorol')) {
            e.target.closest('tr').remove();

            if (userLista_Tbl_Bdy.rows.length === 0) 
            {
                userLista_Tbl_Head.style.display = 'none';
                userLista_Tbl_Bdy.innerHTML = `
                    <tr class="uresSor">
                        <td colspan="5">
                            Nincsenek elmentett béradatok.
                        </td>
                    </tr>
                `;

                oraszam_Tbl_Head.style.display = 'none';
                oraszam_Tbl_Bdy.innerHTML = `
                    <tr class="uresSor">
                        <td colspan="5">
                            Nincs megjeleníthető adat.
                        </td>
                    </tr>
                `;
            }
        }
    });
    

    form_Projekt.querySelector('button[type="submit"]').addEventListener('click', function () {
        document.querySelectorAll('.inpt_Penz').forEach(input => {
            input.value = input.value.replace(/\D/g, '');
        });
    });

    $('#form_Projekt').on('invalid-form.validate', function (event, validator) {

        validator.errorList.forEach(function (error) {

            const reszletek = error.element.closest('.reszletek_Div');

            if (reszletek) {
                reszletek.classList.add('elohiv');

                const cim = reszletek.previousElementSibling;
                cim?.querySelector('.forgoNyil')?.classList.add('srehen');
            }
        });

    });

    form_Projekt.addEventListener('submit', function (e) {
        const resztvevok = [];

        const sorok = userLista_Tbl_Bdy.querySelectorAll('tr:not(.uresSor)');

        for (const tr of sorok) {

            const oraber = tr.querySelector('.userOraber').value;
            const napiber_Ft = tr.querySelector('.userNapiber_Ft').value;
            const napiber_Ora = tr.querySelector('.userNapiber_Ora').value;
            const nev = tr.querySelector('.userNev').textContent.trim();

            if (!oraber) {
                if (!napiber_Ft || !napiber_Ora) {
                    e.preventDefault();
                    alert(`Hiányzó béradat a(z) ${nev} sorban!`);
                    return;
                }
            }

            resztvevok.push({
                Id: tr.dataset.id || null,
                UserId: tr.dataset.userId || null,
                Nev: nev,
                Oraber: oraber,
                Napiber_Ft: napiber_Ft,
                Napiber_Ora: napiber_Ora
            });
        }        
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


    ugyesMegtekint_lbl.addEventListener('click', function(){
        ugyesMegtekint_lbl.classList.add('elrejt');
        ugyesElrejt_lbl.classList.add('elohiv');
        ugyeskedok_Tartalom.classList.add('elohiv');
    });

    ugyesElrejt_lbl.addEventListener('click', function(){
        ugyesMegtekint_lbl.classList.remove('elrejt');
        ugyesElrejt_lbl.classList.remove('elohiv');
        ugyeskedok_Tartalom.classList.remove('elohiv');
    });



    Egyeb_reszKolt_Edit.addEventListener('input',function(){
        const koltseg = Number(inpt_berKoltseg.value.replace(/\D/g,'') || 0) + Number(Egyeb_reszKolt_Edit.value.replace(/\D/g,'') || 0);
        Koltseg_Edit.value = penzFormaz(koltseg);
        const profit = Number(Bevetel_Edit.value.replace(/\D/g,'') || 0) - Number(Koltseg_Edit.value.replace(/\D/g,'') || 0);
        Profit_Edit.value = penzFormaz(profit);
    });

    Koltseg_Edit.addEventListener('input',function(){
        const egyebKolt = Number(Koltseg_Edit.value.replace(/\D/g,'') || 0) - Number(inpt_berKoltseg.value.replace(/\D/g,'') || 0);
        Egyeb_reszKolt_Edit.value = penzFormaz(egyebKolt);
        const profit = Number(Bevetel_Edit.value.replace(/\D/g,'') || 0) - Number(Koltseg_Edit.value.replace(/\D/g,'') || 0);
        Profit_Edit.value = penzFormaz(profit);
    });

    Bevetel_Edit.addEventListener('input',function(){
        const profit = Number(Bevetel_Edit.value.replace(/\D/g,'') || 0) - Number(Koltseg_Edit.value.replace(/\D/g,'') || 0);
        Profit_Edit.value = penzFormaz(profit);
    });

    
});