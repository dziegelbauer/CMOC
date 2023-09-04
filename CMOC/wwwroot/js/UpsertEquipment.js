$("#newTypeForm").submit((event) => {
    event.preventDefault();
    const newName = $("#typeName").val();

    fetch("/api/v1/EquipmentType", {
        method: 'POST',
        headers: new Headers({'content-type': 'application/json'}),
        body: JSON.stringify({
            id: 0,
            name: newName
        })
    }).then(response => {
        response.json().then(data => {
            $("#Equipment_TypeId").append(new Option(data.name, data.id));
        });
        const dialog = new bootstrap.Modal('#newTypeDialog');
        $("#typeName").val('');
        dialog.hide();
    });
});