var productos;
$(document).ready(function () {

    $.ajax({
        url: "getProductos",
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        datatype: 'text',
        success: function (data) {
            // return values 
            var obj = jQuery.parseJSON(data);
            productos =  obj;
        },
        error: function () { console.log('error!!' + error); }
    });

});

$('#btnAgregar').click(function () {
    console.log("HOLA-"+productos.length);
    var idProducto = $('#selectProductos').val();
    var cantidad = $('#cantidadProductos').val();
    var costoProducto;
    var costoTotal;
    var valHidden = $('#productolista').val();
    if (valHidden.length == 0) {
        $('#productolista').val(idProducto+"-"+cantidad);
    }
    else {
        $('#productolista').val(valHidden + ";" + idProducto + "-" + cantidad);
    }
    alert($('#productolista').val());
    for (var i = 0; i < productos.length; i++) {
        if (productos[i].producto_id == idProducto) {
            costoProducto = productos[i].precio;
            costoTotal = parseInt(costoProducto) * parseInt(cantidad);

            $('#tablaProductos').append('<tr><td>' + productos[i].nombre + '</td><td>' + costoProducto + '</td><td>' + cantidad + '</td><td>' + costoTotal + '</td></tr>');

        }
    }
});