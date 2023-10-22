import ApiStatus from "../../../components/ApiStatus.tsx";
import Table from "../../../components/table/Table.tsx";
import {TableColumnHeader} from "../../../types/TableColumnHeader.ts";
import {Equipment} from "../../../types/Equipment.ts";
import {useDeleteEquipment, useFetchEquipment} from "../../../hooks/EquipmentHooks.ts";

const EquipmentList = () => {
    const { data, status, isSuccess } = useFetchEquipment();
    const deleteEquipmentMutation = useDeleteEquipment();

    const tableHeaders: TableColumnHeader[] = [
        { label: "Id", accessor: "id", sortable: true },
        { label: "Type", accessor: "typeName", sortable: true },
        { label: "Serial Number", accessor: "serialNumber", sortable: true },
        { label: "Notes", accessor: "notes", sortable: false },
        { label: "", accessor: "", sortable: false }
    ];
    
    const onDeleteClick = (item: Equipment) => {
        if (window.confirm(`Are you sure you want to delete "${item.typeName} sn: ${item.serialNumber}"?`))
            deleteEquipmentMutation.mutate(item);
    }

    return(
        <>
            {!isSuccess && <ApiStatus status={status}/>}
            {isSuccess &&
                <Table<Equipment>
                    columnHeaders={tableHeaders}
                    data={data}
                    title="Equipment"
                    caption="Manage equipment"
                    onDelete={onDeleteClick}
                />
            }
        </>
    );
}

export default EquipmentList;