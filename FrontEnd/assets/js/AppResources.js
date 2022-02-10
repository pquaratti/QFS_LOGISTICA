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
    var _sHTML = "     "
    _sHTML += "<div class='mt-2 tx-md'>";
    _sHTML += " Redireccionando";
    _sHTML += "</div> ";
    _sHTML += "<div class='mt-4'> ";
    _sHTML += " <div class='spinner-border' role='status'></div>";
    _sHTML += " <span></span>";
    _sHTML += "</div> ";
    _sHTML += "<div class='mt-1'>" + pMensaje + "</div>";
    
    swal({
        title: "",
        html: true,
        text: _sHTML,
        showSpinner: true,
        showCancelButton: false,
        showConfirmButton: false,
    });
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

function APP_SendModalFormAjax(formularioSend, callbackFunction) {
    
    if ($(formularioSend).valid() === false) {
        return false;
    }

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



function TbEdit(control) {
    var urlRedirect = $(control).attr("data-url-redirect");
    window.location.href = urlRedirect;
}

function TbDelete(control) {
    var urlPost = $(control).attr("data-url-redirect");
    var rowDelete = $(control).attr("data-delete-row");
    var refreshdata = $(control).hasClass("refresh-data");
    var noBorrar = $(control).hasClass("no-borrar");
    var _control = $(control);
    
    APP_SweetAlert_Question('Desea desactivar el registro?', 'Haga click en si para desactivar', function () {
        $.ajax({
            type: 'POST',
            url: urlPost,
            data: {},
            success: function (result) {
                if (result.Result.Success === true) {
                    swal("Confirmado!", "El registro se ha eliminado exitosamente!", "success");
                    //$(_control).parent().parent().remove();

                    if (noBorrar === false) {
                        $(_control).parent().parent().fadeOut(300, function () { $(this).remove(); });
                    }

                    if (refreshdata === true) {
                        refreshData();
                    }
                }
                else {
                    swal("Ocurrió un inconveniente", "No se pudo desactivar el registro \n " + result.Result.Message.toString(), "warning");
                }
            },
            error: function (errorResult) {
                console.log(errorResult);
            },
            complete: function () {

            }
        });
    });
}

function TbRecuperar(control) {
    var urlPost = $(control).attr("data-url-redirect");
    var refreshdata = $(control).hasClass("refresh-data");
    var rowDelete = $(control).attr("data-delete-row");
    var _control = $(control);

    APP_SweetAlert_Question('Desea activar el registro?', 'Haga click en si para activar', function () {
        $.ajax({
            type: 'POST',
            url: urlPost,
            data: {},
            success: function (result) {
                if (result.Result.Success === true) {
                    swal("Confirmado!", "El registro se ha activado exitosamente!", "success");
                    //$(_control).parent().parent().remove();
                    //$(_control).parent().parent().fadeOut(300, function () { $(this).remove(); });

                    if (refreshdata === true) {
                        refreshData();
                    }
                }
                else {
                    swal("Ocurrió un inconveniente", "No se pudo activar el registro", "warning");
                }
            },
            error: function (errorResult) {
                console.log(errorResult);
            },
            complete: function () {

            }
        });
    });
}


$('.op-grid-edit').click(function (e) {
    e.preventDefault();
    var urlRedirect = $(this).attr("data-url-redirect");
    window.location.href = urlRedirect;
});

$('.op-grid-delete').click(function (e) {
    e.preventDefault();
    var urlPost = $(this).attr("data-url-redirect");
    var rowDelete = $(this).attr("data-delete-row");
    var _control = $(this).parent().parent();
    
    APP_SweetAlert_Question('Desea desactivar el registro?', 'Haga click en si para desactivar', function () {
        $.ajax({
            type: 'POST',
            url: urlPost,
            data: {},
            success: function (result) {
                if (result.Result.Success === true) {
                    swal("Confirmado!", "El registro se ha desactivado exitosamente!", "success");
                    $(_control).fadeOut(300, function () { $(_control).remove(); });
                }
                else {
                    swal("Ocurrió un inconveniente", "No se pudo desactivar el registro", "warning");
                }
            },
            error: function (errorResult) {
                console.log(errorResult);
            },
            complete: function () {

              //  alert('Borra');
            }
        });
    });

});


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

$('#formularioABM, .formularioABM, .formularioPOST').on('submit', function (e) {
    e.preventDefault();

    var _notifyMessage = $(this).hasClass("formNotify");
    var _redirectForm = $(this).hasClass("formRedirect");
    var _validarForm = $(this).hasClass("formValidar");

    //debugger

    if (_validarForm) {
        if ($(this).valid() === false) {
            return false;
        }
    }

    ShowPreloader();

    APP_SendFormAjax($(this), function (resp) {
        console.log(resp);

        HidePreloader();

        if (resp.Result.Success === true) {
            // Graba bien

            if (resp.Result.RedirectNew === true) {
                if (_notifyMessage) {
                    APP_ShowNotify('Confirmado', resp.Result.Message, notifyTypes.Success);

                    if (_redirectForm) {
                        setTimeout(function () { window.location = resp.Result.urlRedirect; }, 600);
                    }

                }
                else {
                    APP_SweetAlert_ShowMessageSuccessCallback('Confirmado', resp.Result.Message, function () {
                        setTimeout(function () { window.location = resp.Result.urlRedirect; }, 500);
                    });
                }
            }
            else {
                APP_SweetAlert_ShowMessageSuccess('Confirmado', resp.Result.Message);
            }
        }
        else {
            // No graba, mostrar mensaje
            APP_SweetAlert_ShowMessageError('Ingreso', resp.Result.Message);
        }
    });
});

//$('.formularioABM').on('submit', function (e) {
//    e.preventDefault();

//    APP_SendFormAjax($(this), function (resp) {
//        console.log(resp);

//        if (resp.Result.TokenExist === false) {
//            //Redirecciona al login
//        }
//        else {
//            if (resp.Result.Success === true) {
//                // Graba bien

//                if (resp.Result.RedirectNew === true) {
//                    APP_SweetAlert_ShowMessageSuccessCallback('Confirmado', resp.Result.Message, function () {
//                        setTimeout(function () { window.location = resp.Result.urlRedirect; }, 500);
//                    });
//                }
//                else {
//                    APP_SweetAlert_ShowMessageSuccess('Confirmado', resp.Result.Message);
//                }
//            }
//            else {
//                // No graba, mostrar mensaje
//                APP_SweetAlert_ShowMessageError('Ingreso', resp.Result.Message);
//            }
//        }
//    });

//});


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
                    "searching": false,
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

            if ($(elementID).find('.datatablePartial-scrollbar-modal-nosearch').length > 0) {
                $($(elementID).find('.datatablePartial-scrollbar-modal-nosearch')).each(function () {
                    var elementDatatableJS = $(this).DataTable({
                        "scrollY": "200px",
                        "paging": false,
                        "searching":false,
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

function APP_LoadSelectModal(elementID, urlData, filterID) {

    var _dataSend = { "filterID": filterID };
    var _selectElement = $(elementID);
    _selectElement.empty();

    $.getJSON(urlData, _dataSend, function (data) {
        if (!data) {
            return;
        }
        //_selectElement.append($('<option></option>').val('0').text('Please select'));

        $.each(data, function (index, item) {
            _selectElement.append($('<option></option>').val(item.Value).text(item.Text));
        });

        if ($(elementID).find('.select2Modal').length > 0) {
            $(elementID).find('.select2Modal').select2({
                dropdownParent: $('.modal-body'),
                width: '100%'
            });

        }

    });

}

function APP_LoadSelect(elementID, urlData, filterID) {

    var _dataSend = { "filterID": filterID };
    var _selectElement = $(elementID);
    _selectElement.empty();
    _selectElement.append($('<option></option>').val('0').text('Buscando..'));

    $.getJSON(urlData, _dataSend, function (data) {
        if (!data) {
            return;
        }
        //_selectElement.append($('<option></option>').val('0').text('Please select'));
        _selectElement.empty();

        $.each(data, function (index, item) {
            _selectElement.append($('<option></option>').val(item.Value).text(item.Text));
        });

        $(_selectElement).select2();

    });
}

function APP_LoadSelectFilters(elementID, urlData, filterObject) {

    var _selectElement = $(elementID);
    _selectElement.empty();
    _selectElement.append($('<option></option>').val('0').text('Buscando..'));

    $.getJSON(urlData, filterObject, function (data) {
        if (!data) {
            return;
        }
        //_selectElement.append($('<option></option>').val('0').text('Please select'));
        _selectElement.empty();

        $.each(data, function (index, item) {
            _selectElement.append($('<option></option>').val(item.Value).text(item.Text));
        });

        $(_selectElement).select2();

    });
}

function APP_LoadSelectCallback(elementID, urlData, filterID , callbackFunction) {

    var _dataSend = { "filterID": filterID };
    var _selectElement = $(elementID);
    _selectElement.empty();

    $.getJSON(urlData, _dataSend, function (data) {
        if (!data) {
            return;
        }
        //_selectElement.append($('<option></option>').val('0').text('Please select'));

        $.each(data, function (index, item) {
            _selectElement.append($('<option></option>').val(item.Value).text(item.Text));
        });
        
        callbackFunction();
    });

    //https://www.codeproject.com/Tips/883701/An-ASP-NET-MVC-Cascading-Dropdown-List

    //https://dotnetfiddle.net/PBi075
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

function APP_Datatable_Async(idDatatable, ActionName, methodName, functionDataSend, arrayColumns) {
    var dtAsync = $(idDatatable).dataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "searching": true,
        "ajax": {
            "url": ActionName,
            "type": methodName,
            "datatype": "json",
            "data": functionDataSend
        },
        "columns": arrayColumns,
        "language": {
            "url": '../assets/Spanish.json'
        }
    });
    
    return dtAsync;
}

function APP_Datatable_Ajax(objConfig) {
    var dtAsync = $(objConfig.tableID).dataTable({
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "searching": true,
        "rowId": objConfig.rowId,
        "ajax": {
            "url": objConfig.ajaxURL,
            "type": 'GET',
            "datatype": "json",
            "data": objConfig.customFunction
        },
        "columns": objConfig.arrayColumns,
        "language": {
            "url": '../assets/Spanish.json'
        }
    });

    return dtAsync;
}

function executeAjaxPostReport(paramUrl, paramData, paramFunctionCallback) {
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
            //$.magnificPopup.close();
        }
    });
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

function APP_OpenModalForm(elementID,elementIDForm, objectData, urlPartialView)
{
    $('.modal').modal('hide');

    $(elementID).html('');
    ShowPreloader();
    APP_LoadPartialViewCallback(elementID, objectData, urlPartialView, function () {
        HidePreloader();
        $(elementIDForm).modal();
    });   
}

function APP_OpenModalFormDefault(objectData, urlPartialView) {
    APP_OpenModalForm('#viewModalForm', '#modalFormDefault', objectData, urlPartialView);
}

function APP_OpenModalFormCallback(elementID, elementIDForm, objectData, urlPartialView, callbackFunction) {

    $('.modal').modal('hide');

    $(elementID).html('');
    ShowPreloader();
    APP_LoadPartialViewCallback(elementID, objectData, urlPartialView, function () {
        HidePreloader();
        $(elementIDForm).modal();
        callbackFunction();
    });
}

function APP_SaveModalForm(formularioID, functionCallback) {
    
    if ($(formularioID).valid() === false) {
        return false;
    }

    APP_SendFormAjax($(formularioID), function (resp) {
        if (resp.Result.Success === true) {
            APP_ShowNotify('Confirmado', resp.Result.Message, notifyTypes.Success);
            $(formularioID).find('.btn-cerrar').click();
            functionCallback();
        }
        else
        {
            APP_ShowNotify('Alerta', resp.Result.Message, notifyTypes.Error);

        }

    });
    
}

function APP_SelectValueComboSelect2(idcontrol, value)
{
    $(idcontrol).val(value);
    $(idcontrol).change();
}

// OPCIONES DE GRID VIEW (DATATABLES)
function APP_GRID_Activo(value) {
    var htmlControls = '';

    if (value === true) {
        htmlControls = '<span class="text-md text-success"><i class="fa fa-check-circle"></i></span>';
    }
    else {
        htmlControls = '<span class="text-md text-danger"><i class="fa fa-times-circle"></i></span>';
    }

    return htmlControls;
}


function APP_GRID_Options_Buttons(id, estado, options) {
    var htmlControls = '';

    if (options.modal_abm === true) {
        htmlControls += '<button class="btn btn-info btn-sm btn-grid btnModalABM" data-modal-content="' + options.modal_content + '" data-modal-form="' + options.modal_form + '" data-modal-geturl="' + options.modal_geturl + '" data-modal-value="' + id +'"><i class="fa fa-edit"></i></button>';
    }
    else
    {
        htmlControls += '<button class="btn btn-info btn-sm btn-grid op-grid-edit" data-url-redirect="' + options.urlEdit + '/' + id + '" onclick="TbEdit(this);"><i class="fa fa-edit"></i></button>';
    }

    if (estado === true) {
        htmlControls += '<button type="button" data-id-rel="' + id + '" url-delete="' + options.urlDelete +'" class="btn btn-danger btn-sm btn-grid btn-borrar-detalle ml-2" title="Borrar item" ><i class="fa fa-trash"></i></button>';
    }
    else {
        htmlControls += '<button class="btn btn-success btn-sm op-grid-recuperar ml-2 refresh-data btn-grid" data-url-redirect="' + options.urlActivar + '/' + id + '" onclick="TbRecuperar(this);">Activar</button>'
    }
    

    return htmlControls;
}


function APP_GRID_Options_Dropdownlist(id, estado, options) {
    var htmlControls = '';

    htmlControls += '<div class="dropdown">';
    htmlControls += '   <button class="btn btn-xs btn-white dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Acciones</button>';
    htmlControls += '   <div class="dropdown-menu tx-14" aria-labelledby="dropdownMenuButton">';

    if (options.modal_abm === true) {
        htmlControls += '<button class="dropdown-item btnModalABM" data-modal-content="' + options.modal_content + '" data-modal-form="' + options.modal_form + '" data-modal-geturl="' + options.modal_geturl +'" data-modal-value="' + id +'"> ';
        htmlControls += '<i class="fa fa-edit"></i> Editar</button>';
    }
    else {
        htmlControls += '   <button class="dropdown-item" data-url-redirect="' + options.urlEdit + '/' + id + '" onclick="TbEdit(this);"><i class="fa fa-edit"></i> Editar</button>';
    }

    if (estado === true) {
        htmlControls += '       <button class="dropdown-item" data-url-redirect="' + options.urlDelete + '/' + id + '" onclick="TbDelete(this);"><i class="fa fa-times-circle"></i> Eliminar</button> ';
    }
    else {
        htmlControls += '       <button class="dropdown-item" data-url-redirect="' + options.urlActivar + '/' + id + '" onclick="TbDelete(this);"><i class="fa fa-check"></i> Reactivar</button> ';
    }
    
    htmlControls += '       <button class="dropdown-item" ><i class="fa fa-print"></i> Imprimir</button> ';
    htmlControls += '   </div> ';
    htmlControls += '</div>';

    return htmlControls;
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


$('body').on('click', '.btnModalABM', function (e) {
    e.preventDefault();

    //var _viewModalContent = $(this).attr("data-modal-content");
    //var _viewModalForm = $(this).attr("data-modal-form");
    //var _urlModalGet = $(this).attr("data-modal-geturl");
    //var _dataModalValue = $(this).attr("data-modal-value");

    var _viewModalContent = objConfigABM.modal_content;
    var _viewModalForm = objConfigABM.modal_form;
    var _urlModalGet = objConfigABM.modal_geturl;
    var _dataModalValue = $(this).attr('data-modal-value');

    APP_OpenModalFormCallback(_viewModalContent, _viewModalForm, { id: _dataModalValue }, _urlModalGet,
        function (res) {
            // LIBRE PARA EJECUTAR CUANDO FINALIZA EL PROCESO
        }
    );
});


$('body').on('submit', '.formularioABMModal', function (e) {
    e.preventDefault();

    var _paramRefreshDatatable = $(this).attr('refreshDatatable');
    var _paramResetForm = $(this).attr('resetForm');
    var _paramCloseForm = $(this).attr('closeForm');
    var _functionExtra = $(this).attr('funcionExtra');
    var _functionExtraResponse = $(this).attr('funcionExtraResponse');

    ShowPreloader();

    APP_SendModalFormAjax($(this), function (res) {

        HidePreloader();

        if (res.Result.Success === true) {
            APP_SweetAlert_ShowMessageSuccessCallback('Exitoso!', res.Result.Message, function () {
                if (_paramRefreshDatatable === "true")
                {
                    refreshData();
                }

                if (_paramResetForm === "true") {
                    resetForm();
                }

                if (_paramCloseForm === "true") {
                    $('.modal').modal('hide');
                }

                if (_functionExtra === "true") {
                    functionExtra();
                }

                if (_functionExtraResponse === "true") {
                    functionExtraResponse(res);
                }
                
            });
        }
        else {
            APP_SweetAlert_ShowMessageError('Ocurrió un inconveniente', res.Result.Message);
        }

    });
});


$('body').on('click', '.btnOpenModal', function (e) {
    e.preventDefault();

    var _URLPartial = $(this).attr('modal-url');
    var _dataValuePrimary = $(this).attr('modal-value-primary');
    var _dataValueSecondary = $(this).attr('modal-value-secondary');
    var _dataAux = $(this).attr('modal-value-extra');
    var _functionExtra = $(this).attr('modal-funcionExtra');
    
    var _parametros;

    if (_dataAux === undefined) {
        _parametros = {
            cabeceraID: _dataValuePrimary,
            detalleID: _dataValueSecondary
        };
    }
    else {
        _parametros = {
            cabeceraID: _dataValuePrimary,
            detalleID: _dataValueSecondary,
            subdetalleID: _dataAux
        };
    }

    APP_OpenModalFormCallback(
        '#viewModalForm',
        '#modalFormDefault',
        _parametros,
        _URLPartial,
        function (res) {
            // LIBRE PARA EJECUTAR CUANDO FINALIZA EL PROCESO
            if (_functionExtra === "true") {
                functionExtra();
            }
        }
    );

});

function APP_CloseOpenModal(parametros, url) {
    $('.btn-cerrar').click();

    setTimeout(function () {
        APP_OpenModalFormCallback(
            '#viewModalForm',
            '#modalFormDefault',
            parametros,
            url,
            function (res) {

                // LIBRE PARA EJECUTAR CUANDO FINALIZA EL PROCESO
            }
        );
    }, 1000);

}


$('body').on('click', '.btn-borrar-detalle', function (e) {
    e.preventDefault();

    var _idRel = $(this).attr('data-id-rel');
    var _url = $(this).attr('url-delete');
    var _functionExtra = $(this).attr('delete-funcionExtra');

    var _renglon = $(this).parent().parent();

    APP_SweetAlert_Question('Eliminar item', 'Si desea eliminar el item haga click en Sí', function () {
        APP_executeAjaxPostNoClosePopup(_url, { borrarID: _idRel }, function (res) {
            if (res.Result.Success === true) {
                $(_renglon).fadeOut('slow');
                APP_SweetAlert_ShowMessageSuccess('Operación exitosa!', 'Se ha eliminado el item exitosamente');

                if (_functionExtra === "true") {
                    functionExtraDelete();
                }
            }
            else {
                APP_SweetAlert_ShowMessageError('Ocurrió un inconveniente', res.Result.Message);
            }
        }); 
    });
});


function APP_Control_Solo_Lectura(control){
    $(control).attr("readonly", "readonly");
}

function APP_Control_Habilitar_Edicion(control) {
    $(control).removeAttr("readonly");
}

function APP_SaveFormCustom(objeto, urlSave, callbackFunction) {
    var fd = new FormData();
    fd.append('CustomObject', JSON.stringify(objeto));

    $.ajax({
        type: 'POST',
        url: urlSave,
        data: fd,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.Result.Success === true) {
                APP_SweetAlert_ShowMessageSuccessCallback('Exitoso!', res.Result.Message, function () {
                    callbackFunction();
                    $('.modal').modal('hide');
                });
            }
            else {
                APP_SweetAlert_ShowMessageError('Ocurrió un inconveniente', res.Result.Message);
            }
        },
        error: function (errorResult) {
            console.log(errorResult);
        },
        complete: function () {

        }
    });
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


function ButtonModalTableAjaxEdit(valuePrimary, valueSecondary, valueExtra, urlModal) {
    var htmlControl = '';

    htmlControl += '<button type="button"';
    htmlControl += '    modal-value-primary="' + valuePrimary + '" ';
    htmlControl += '    modal-value-secondary="' + valueSecondary + '" ';
    htmlControl += '    modal-value-aux="' + valueExtra + '" ';
    htmlControl += '    modal-url="' + urlModal + '" ';
    htmlControl += '    class="btn btn-info btn-sm btn-grid btnOpenModal" title="">';
    htmlControl += '<i class="fa fa-edit"></i> ';
    htmlControl += '</button>';

    return htmlControl;
}


$('body').on('click', '.btn-question-action', function (e) {
    e.preventDefault();

    var _idRel = $(this).attr('data-id-rel');
    var _url = $(this).attr('accion-url');
    var _nombreAccionClave = $(this).attr('accion-nombre-clave');
    var _tituloModal = $(this).attr('accion-titulo-modal');
    var _mensajeModal = $(this).attr('accion-mensaje-modal');
    var _mensajeExitoso = $(this).attr('accion-mensaje-exitoso');

    APP_SweetAlert_Question(_tituloModal, _mensajeModal, function () {
        APP_executeAjaxPostNoClosePopup(_url, { accionID: _idRel }, function (res) {
            if (res.Result.Success === true) {
                APP_SweetAlert_ShowMessageSuccess('Operación exitosa!', _mensajeExitoso);
                functionAccionCustom(_nombreAccionClave);
            }
            else {
                APP_SweetAlert_ShowMessageError('Ocurrió un inconveniente', res.Result.Message);
            }
        });
    });
});


function APP_DatatableExtend_Async(idDatatable, ActionName, methodName, functionDataSend, arrayColumns, rowCallbackFunction) {

    var _Search = true;

    if ($(idDatatable).hasAttr('nosearch')) {
        _Search = false;
    }

    var dtAsync = $(idDatatable).dataTable({
        "language": {
            "url": _urlDatatableJSSpanish
        },
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "searching": _Search,
        "ajax": {
            "url": ActionName,
            "type": methodName,
            "datatype": "json",
            "data": functionDataSend
        },
        "columns": arrayColumns,
        "rowCallback": function (row, data) {
            rowCallbackFunction(row, data);
        }
    });

    return dtAsync;
}

function ShowMessageLoadingProcessPending(pElemento, pTextoLoader) {
    var sHTML = '';
    sHTML += ' <div> ';
    sHTML += '      <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
    sHTML += '      ' + pTextoLoader ;
    sHTML += '</div>';

    $(pElemento).html(sHTML);
}

function ShowMessageLoadingProcessSuccess(pElemento) {
    var sHTML = '<p class="tx-success"> <i class="fa fa-check"></i> Datos procesados exitosamente</p>';
    $(pElemento).html(sHTML);
}

function ShowMessageLoadingProcessFail(pElemento) {
    var sHTML = '<p class="tx-danger"> <i class="fa fa-times"></i> Ocurrió un inconveniente al procesar los datos</p>';
    $(pElemento).html(sHTML);
}

function CreateDonutMorrisChart(unElemento) {
    new Morris.Donut({
        element: unElemento,
        data: $('#' + unElemento).data('print'),
        formatter: function (x) { return x + "%" },
        resize: true,
        colors: $('#' + unElemento).data('print2')
    });
}

function CreateBarMorrisChart(unElemento) {
    new Morris.Bar({
        element: unElemento,
        data: $('#' + unElemento).data('print'),
        xkey: 'Nombre',
        ykeys: ['Promedio'],
        barColors: ['#560bd0'],
        gridLineColor: '#e5e9f2',
        gridStrokeWidth: 1,
        gridTextSize: 11,
        hideHover: 'auto',
        barSizeRatio: 0.20,
        resize: true
    });
}

function CreateBarMorrisChartTotal(unElemento) {
    new Morris.Bar({
        element: unElemento,
        data: $('#' + unElemento).data('print'),
        xkey: 'Nombre',
        ykeys: ['Total'],
        barColors: ['#560bd0'],
        gridLineColor: '#e5e9f2',
        gridStrokeWidth: 2,
        gridTextSize: 12,
        hideHover: 'auto',
        barSizeRatio: 0.20,
        resize: true
    });
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



$('body').on('submit', '.formularioABMModalFiles', function (e) {
    e.preventDefault();

    var _paramRefreshDatatable = $(this).attr('refreshDatatable');
    var _paramResetForm = $(this).attr('resetForm');
    var _paramCloseForm = $(this).attr('closeForm');
    var _functionExtra = $(this).attr('funcionExtra');

    ShowPreloader();
    console.log("estoy entrando");
    APP_SendModalFormAjaxFiles($(this), function (res) {

        HidePreloader();

        if (res.Result.Success === true) {
            APP_SweetAlert_ShowMessageSuccessCallback('Exitoso!', res.Result.Message, function () {
                if (_paramRefreshDatatable === "true") {
                    refreshData();
                }

                if (_paramResetForm === "false") {
                    resetForm();
                }

                if (_paramCloseForm === "true") {
                    $('.modal').modal('hide');
                }

                if (_functionExtra === "true") {
                    functionExtra();
                }

            });
        }
        else {
            APP_SweetAlert_ShowMessageError('Ocurrió un inconveniente', res.Result.Message);
        }

    });
});

$('body').on('submit', '.formularioABMFiles', function (e) {
    e.preventDefault();

    var _paramRefreshDatatable = $(this).attr('refreshDatatable');
    var _paramResetForm = $(this).attr('resetForm');
    var _functionExtra = $(this).attr('funcionExtra');
    var _refreshData = $(this).attr('refreshData');
    ShowPreloader();

    APP_SendModalFormAjaxFiles($(this), function (res) {

        HidePreloader();

        if (res.Result.Success === true) {
            APP_SweetAlert_ShowMessageSuccessCallback('Exitoso!', res.Result.Message, function () {
                if (_paramRefreshDatatable === "true") {
                    refreshData();
                }

                if (_refreshData === "true") {
                    refreshData();
                }

                if (_paramResetForm === "false") {
                    resetForm();
                }

                if (_functionExtra != null) {
                    functionExtraParametro(_functionExtra);
                }

            });
        }
        else {
            APP_SweetAlert_ShowMessageError('Ocurrió un inconveniente', res.Result.Message);
        }

    });
});


function resetForm() {

}


function APP_SendModalFormAjaxFiles(formularioSend, callbackFunction) {

    if ($(formularioSend).valid() === false) {
        return false;
    }

    var dataSendForm = new FormData($(formularioSend)[0]);
    //var fileUpload = document.getElementById('fileUpload');
    var fileUpload = $(formularioSend).find('input:file')[0];

    console.log(fileUpload);
    //Iterating through each files selected in fileInput
    for (i = 0; i < fileUpload.files.length; i++) {
        //Appending each file to FormData object
        console.log(fileUpload.files[i].name);
        console.log(fileUpload.files[i]);
        dataSendForm.append(fileUpload.files[i].name, fileUpload.files[i]);
    }


    _urlSend = $(formularioSend).attr('action');

    $.ajax({
        type: 'POST',
        url: _urlSend,
        contentType: false,
        processData: false,
        data: dataSendForm,
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

function getParameterByName(name) {
    var regexS = "[\\?&]" + name + "=([^&#]*)",
        regex = new RegExp(regexS),
        results = regex.exec(window.location.search);
    if (results == null) {
        return "";
    } else {
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
}

function ClickRedirect(pControl) {
    APP_SweetAlert_MessageProcess('');
    window.location = $(pControl).attr('url');
}
