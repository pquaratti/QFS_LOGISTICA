var elementsDatatables = [];

var buttonColorTypes = {
    Primary: 'btn-primary',
    Success: 'btn-success',
    Info: 'btn-info',
    Warning: 'btn-warning',
    Danger: 'btn-danger'
};

var notifyTypes = {
    Success: 'Success',
    Warning: 'Warning',
    Error: 'Error',
    Info: 'Info'
};

function APP_RedirectTo(url) {
    location.href = url;
}

function APP_PopulateJS(urlJson, selectControlId) {
    fetch(urlJson).then(function (result) {
        if (result.ok) {
            return result.json();
        }
    }).then(function (data) {
        data.forEach(function (element) {
            let miSelect = document.getElementById(selectControlId);
            let opt = document.createElement("option");
            opt.appendChild(document.createTextNode(element.Name));
            opt.value = element.Value;

            miSelect.appendChild(opt);
        });
    });
}

function APP_SweetAlert_ShowAlert(titulo, contenido) {
    swal(titulo, contenido, "info");
}


function APP_SweetAlert_ShowMessageSuccess(titulo, contenido) {
    swal(titulo, contenido, "success");
}

function APP_SweetAlert_ShowMessageSuccessCallback(titulo, contenido, callback) {
    swal({
        title: titulo,
        text: contenido,
        type: "success"
    }, function () {
        callback();
    });
}


function APP_SweetAlert_ShowMessageError(titulo, contenido) {
    swal(titulo, contenido, "error");
}

function APP_SweetAlert_ShowMessageWarning(titulo, contenido, buttonTextYes, callback) {
    swal({
        title: titulo,
        text: contenido,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: buttonTextYes,
        closeOnConfirm: false
    }, function () {
        callback();
    });
}

function APP_SweetAlert_Question(titulo, contenido, callbackFunction) {
    swal({
        title: titulo,
        text: contenido,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                callbackFunction();
            } else {
                //swal("Cancelled", "Your imaginary file is safe :)", "error");
            }
        });
}


function APP_SweetAlert_ShowForm(idForm, urlForm) {
    swal({
        title: "HTML <small>Title</small>!",
        text: "<input type='text'><label class='my-label'>Name</label>",
        html: true
    });

    $.get(urlForm, function (data) {
        console.log(data);


    });
}

function APP_SweetAlert_MessageProcess(pMensaje) {
    swal(pMensaje);
}

function APP_executeAjaxPostNoClosePopup(paramUrl, paramData, paramFunctionCallback) {
    $.ajax({
        type: 'POST',
        url: paramUrl,
        data: paramData,
        success: function (response) {
            paramFunctionCallback(response);
        },
        error: function (errorResult) {
            console.log(errorResult);
        },
        complete: function () {

        }
    });
}

function APP_executeAjax(methodName,paramUrl, paramData, paramFunctionCallback) {
    $.ajax({
        type: methodName,
        url: paramUrl,
        data: paramData,
        success: function (response) {
            paramFunctionCallback(response);
        },
        error: function (errorResult) {
            console.log(errorResult);
        },
        complete: function () {

        }
    });
}

function APP_SendFormAjax(formularioSend, callbackFunction) {

    dataToPost = $(formularioSend).serialize();
    _urlSend = $(formularioSend).attr('action');

    $.ajax({
        type: 'POST',
        url: _urlSend,
        data: dataToPost,
        success: function (result) {
            callbackFunction(result);
        },
        error: function (errorResult) {
            console.log(errorResult);
        },
        complete: function () {
            
        }
    });
}


$('.op-back-form').click(function (e) {
    e.preventDefault();
    var urlRedirect = $(this).attr("data-url-redirect");
    window.location.href = urlRedirect;
});

$('.op-btn-new').click(function (e) {
    e.preventDefault();
    var urlRedirect = $(this).attr('data-url-redirect');
    window.location.href = urlRedirect;
});

$('body').on('click', '.op-btn-redirect', function (e) {
    e.preventDefault();
    var urlRedirect = $(this).attr('data-url-redirect');
    window.location.href = urlRedirect;
});

$('.op-submit-form').click(function (e) {
    e.preventDefault();

    var _urlSend = $(this.form).attr('action');

    APP_SendFormAjax($(this.form), function (resp) {
        console.log(resp);

        if (resp.Result.TokenExist === false) {
            //Redirecciona al login
        }
        else {
            if (resp.Result.Success === true) {
                // Graba bien
            }
            else
            {
                // No graba, mostrar mensaje
                APP_SweetAlert_ShowMessageError('Ingreso', resp.Result.Message);
            }
        }
    });
    
});

function renderGridBase(viewContent, tableElement, urlAction, objectSearch) {

    $(viewContent).html("<p> Buscando datos ... </p>");
    
    $.ajax({
        type: "GET",
        data: objectSearch,
        url: urlAction,
        contentType: "html",
        success: function (result) {
            $(viewContent).html(result);

            $(tableElement).DataTable({
                "searching": false,
                "ordering": false,
                "scrollY": "400px",
                "paging": false
            });
        }
    });
}

function APP_HTML_Loader(elementID) {
    sHTML = '<div class="mt-4"> <div class="spinner-border" role="status"></div> <span> Cargando </span></div>';
    $(elementID).html(sHTML);
    return sHTML;
}

function APP_LoadPartialView(elementID, objectData, urlPartialView) {
    APP_HTML_Loader(elementID);

    $.ajax({
        type: "GET",
        data: objectData,
        url: urlPartialView,
        contentType: "html",
        success: function (result) {
            $(elementID).html(result);
            $.validator.unobtrusive.parse($(elementID));

            if ($(elementID).find('.datatablePartial').length > 0) {
                $($(elementID).find('.datatablePartial')).DataTable();
            }
            
            if ($(elementID).find('.datatablePartial-scrollbar').length > 0)
            {
                $($(elementID).find('.datatablePartial-scrollbar')).DataTable({
                    "scrollY": "400px",
                    "scrollCollapse": true,
                    "paging": false,
                    "language": {
                        "url": '../assets/Spanish.json'
                    }
                });
            }

            if ($(elementID).find('.datatablePartial-scrollbar-nosearch').length > 0) {
                $($(elementID).find('.datatablePartial-scrollbar-nosearch')).DataTable({
                    "scrollY": "400px",
                    "scrollCollapse": true,
                    "paging": false,
                    "searchable": false,
                    "language": {
                        "url": '../assets/Spanish.json'
                    }
                });
            }

            if ($(elementID).find('.datatablePartial-scrollbar-basic').length > 0) {
                $($(elementID).find('.datatablePartial-scrollbar-basic')).DataTable({
                    "scrollY": "400px",
                    "scrollCollapse": true,
                    "paging": false,
                    "info": false,
                    "language": {
                        "url": '../assets/Spanish.json'
                    }
                });
            }


            if ($(elementID).find('.select2').length > 0) {
                $(elementID).find('.select2').select2();
            }

            if ($(elementID).find('.select2Modal').length > 0) {
                $(elementID).find('.select2Modal').select2({
                    dropdownParent: $('.modal-body'),
                    width: '100%'
                });
            }

            if ($(elementID).find('.select2ModalAjax').length > 0) {
                $(elementID).find('.select2ModalAjax').select2({
                    ajax: {
                        //url: urlSearch,
                        delay: 200,
                        width: 'resolve',
                        data: function (params) {
                            return {
                                q: params.term// search term
                            };
                        },
                        processResults: function (data) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    placeholder: "Escribiendo",
                    minimumInputLength: 3,
                    language: "es",
                    dropdownParent: $('.modal-body'),
                    width: '100%'
                });
            }

        }
    });   
}

function APP_LoadPartialViewCallback(elementID, objectData, urlPartialView, callbackFunction) {
    elementsDatatables = [];

    APP_HTML_Loader(elementID);

    $.ajax({
        type: "GET",
        data: objectData,
        url: urlPartialView,
        contentType: "html",
        success: function (result) {
            $(elementID).html(result);
            $.validator.unobtrusive.parse($(elementID));

            if ($(elementID).find('.datatablePartial').length > 0) {
                $($(elementID).find('.datatablePartial')).DataTable();
            }

            if ($(elementID).find('.datatablePartial-scrollbar').length > 0) {
                $($(elementID).find('.datatablePartial-scrollbar')).DataTable({
                    "scrollY": "300px",
                    "scrollCollapse": true,
                    "paging": false,
                    "language": {
                        "url": '../assets/Spanish.json'
                    }
                });
            }

            if ($(elementID).find('.datatablePartial-scrollbarv2').length > 0) {
                $($(elementID).find('.datatablePartial-scrollbarv2')).DataTable({
                    "scrollY": "300px",
                    "paging": false,
                    "language": {
                        "url": '../assets/Spanish.json'
                    }
                });
            }

            
            if ($(elementID).find('.datatablePartial-scrollbar-modal').length > 0) {
                $($(elementID).find('.datatablePartial-scrollbar-modal')).each(function () {
                    var elementDatatableJS = $(this).DataTable({
                                                "scrollY": "300px",
                                                "paging": false,
                                                "language": {
                                                    "url": '../assets/Spanish.json'
                                                }
                    });

                    elementsDatatables.push(elementDatatableJS);
                });
            }

            if ($(elementID).find('.select2').length > 0) {
                $(elementID).find('.select2').select2();
            }

            if ($(elementID).find('.select2Modal').length > 0) {
                $(elementID).find('.select2Modal').select2({
                    dropdownParent: $('.modal-body'),
                    width: '100%'
                });
            }

            if ($(elementID).find('.select2ModalAjax').length > 0) {
                $(elementID).find('.select2ModalAjax').select2({
                    ajax: {
                        //url: urlSearch,
                        delay: 200,
                        width: 'resolve',
                        data: function (params) {
                            return {
                                q: params.term// search term
                            };
                        },
                        processResults: function (data) {
                            return {
                                results: data.items
                            };
                        },
                        cache: true
                    },
                    placeholder: "Escribiendo",
                    minimumInputLength: 3,
                    language: "es",
                    dropdownParent: $('.modal-body'),
                    width: '100%'
                });
            }

            $(".validarnumero").keypress(function (evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                if (charCode !== 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                    evt.preventDefault();
                }
            });

            APP_SET_InputDate();
            
            callbackFunction();
        }
    });
}

function APP_OpenModalLoading() {
    $('#modalLoading').modal();
}

function APP_CloseModalLoading() {
    $('#btnCloseModalLoading').click();
}

function APP_ShowNotify(pTitle, pMessage, pType) {

    switch (pType) {
        case notifyTypes.Success:
            $.toast({
                heading: pTitle,
                text: pMessage,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: 'success',
                hideAfter: 3000,
                stack: 6
            });
            break;
        case notifyTypes.Error:
            $.toast({
                heading: pTitle,
                text: pMessage,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: 'error',
                hideAfter: 3000
            });
            break;
        case notifyTypes.Warning:
            $.toast({
                heading: pTitle,
                text: pMessage,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: 'warning',
                hideAfter: 3000,
                stack: 6
            });
            break;
        default:
            $.toast({
                heading: pTitle,
                text: pMessage,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: 'info',
                hideAfter: 3000,
                stack: 6
            });
            break;
    }
}

function ShowPreloader() {
    $('#spinnerBackground').css('height', $('.content-page').height());
    $('#spinnerElement').css('margin-top', (($(window).height() / 2) + $(window).scrollTop()));
    $(".preloader").fadeIn();

    console.log('Show preloader')
}

function HidePreloader() {
    $(".preloader").fadeOut();

    console.log('Hide preloader');
}

function BlockElement(element) {
    $(element).block({
        message: null,
        overlayCSS: {
            backgroundColor: 'rgba(0, 0, 0, 0.28)'
        }
    });
}

function UnBlockElement(element) {
    $(element).unblock();
}

function APP_GRID_Date(data) {
    return moment(data).format('DD/MM/YYYY');
}

function APP_SET_InputDate() {
    $('.inputDate').toArray().forEach(function (field) {
        new Cleave(field, {
            date: true,
            datePattern: ['d', 'm', 'Y']
        });
    });
}

function APP_Control_Solo_Lectura(control){
    $(control).attr("readonly", "readonly");
}

function APP_Control_Habilitar_Edicion(control) {
    $(control).removeAttr("readonly");
}


function APP_ValidarControlesContenedor(idContenedor) {
    var esValido = true;

    $(idContenedor + ' .validarTexto').each(function (a, obj) {
        esValido = validarCampoDeTexto($(obj)) && esValido;
    });

    $(idContenedor + ' .validateSelect').each(function (a, obj) {
        esValido = validarSelect($(obj)) && esValido;
    });
}

function validarCampoDeTexto(obj) {
    var esValido = true;
    if (obj.val().trim() === '') {
        obj.addClass('input-error');
        esValido = false;
    } else {
        obj.removeClass('input-error');
    }
    obj.bind("change", function () { validarCampoDeTexto($(this)); });
    return esValido;
}

function CloseModal(callbackFunction) {
    $('.btn-cerrar').click();
    setTimeout(function () { callbackFunction(); }, 1000);
}

function resizeDatatablesJS() {
    elementsDatatables.forEach((eldatatable) => {
        eldatatable.columns.adjust();
    });
}

$(document).on('shown.bs.modal', function () {
    resizeDatatablesJS();
});