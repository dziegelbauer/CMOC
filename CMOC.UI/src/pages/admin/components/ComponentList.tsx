import ApiStatus from "../../../components/ApiStatus.tsx";
import Table from "../../../components/table/Table.tsx";
import {TableColumnHeader} from "../../../types/TableColumnHeader.ts";
import {Component} from "../../../types/Component.ts";
import {useDeleteComponent, useFetchComponents} from "../../../hooks/ComponentHooks.ts";

const ComponentList = () => {
    const { data, status, isSuccess } = useFetchComponents();
    const deleteComponentMutation = useDeleteComponent();
    
    const tableHeaders: TableColumnHeader[] = [
        { label: "Id", accessor: "id", sortable: true },
        { label: "Type", accessor: "typeName", sortable: true },
        { label: "Serial Number", accessor: "serialNumber", sortable: true },
        { label: "Equipment", accessor: "equipment", sortable: true },
        { label: "", accessor: "", sortable: false }
    ]
    
    const onDeleteClick = (item: Component) => {
        if (window.confirm(`Are you sure you want to delete "${item.typeName} sn: ${item.serialNumber}"?`))
            deleteComponentMutation.mutate(item);
    }
    
    return(
        <>
            {!isSuccess && <ApiStatus status={status}/>}
            {isSuccess && 
                <Table<Component> 
                    columnHeaders={tableHeaders} 
                    data={data} 
                    title="Components"
                    caption="Manage components"
                    onDelete={onDeleteClick}
                />
            }
        </>    
    );
}

export default ComponentList;