let dataTable;
$(document).ready( function () {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/v1/Components",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "10%"},
            { "data": "serialNumber", "width": "20%"},
            { "data": "type", "width": "20%"},
            { 
                "data": "operational",
                "render": function (data, type, row, meta ) {
                    return data === true
                        ? `<i class="bi bi-check-circle-fill" style="color: green"></i>`
                        : `<i class="bi bi-x-circle-fill" style="color: red"></i>`
                },
                "width": "10%"
            },
            { "data": "equipment", "width": "20%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                                <a href="/Admin/Components/Upsert?id=${data}" class="btn btn-success text-white mx-2">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a onclick="Delete('/api/v1/Components/' + ${data})" class="btn btn-danger text-white mx-2">
                                    <i class="bi bi-trash-fill"></i>
                                </a>
                           </div>`
                },
                "width": "20%"
            }
        ],
        "width": "100%"
    });
} );

function Delete(url)
{
    swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this component!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    })
        .then((willDelete) => {
            if (willDelete.isConfirmed) {
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        if(data.success) {
                            dataTable.ajax.reload();
                            toastr.success(data.message);
                        }
                    }
                }).Error(message => {
                    toastr.error(message);
                });
            }
        });
}