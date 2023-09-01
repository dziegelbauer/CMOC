var dataTable;
$(document).ready( function () {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/v1/Capabilities",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "20%"},
            { "data": "name", "width": "50%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                                <a href="/Admin/Capabilities/Upsert?id=${data}" class="btn btn-success text-white mx-2">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                                <a onclick="Delete('/api/v1/Capabilities/' + ${data})" class="btn btn-danger text-white mx-2">
                                    <i class="bi bi-trash-fill"></i>
                                </a>
                           </div>`
                },
                "width": "25%"
            }
        ],
        "width": "100%"
    });
} );

function Delete(url)
{
    swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this menu item!",
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