var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/ApplicationUser/GetAll"
        },
        "columns": [
            { "data": "userName", "width": "25%" },
            { "data": "email", "width": "25%" },
            { "data": "firstName", "width": "20%" },
            { "data": "lastName", "width": "20%" },

            {
                "data": "id",
                "render": function (data) {
                    let lockout = new Date(data.lockoutEnd).getTime();
                    let today = new Date().getTime();
                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="fas fa-lock-open"></i>  Unlock
                                </a>
                            </div>
                           `;
                    }
                        else {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="fas fa-lock"></i>  Lock
                                </a>
                            </div>
                           `;
                    }
                    
                }, "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/ApplicationUser/LockUnlock",
        data: JSON.stringify(id),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                toastr.succes(data.message);
                $('#tblData').DataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}

