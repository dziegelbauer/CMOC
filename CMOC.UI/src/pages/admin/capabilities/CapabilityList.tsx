import {useDeleteCapability, useFetchCapabilities} from "../../../hooks/CapabilityHooks.ts";
import ApiStatus from "../../../components/ApiStatus.tsx";
import Table from "../../../components/table/Table.tsx";
import {TableColumnHeader} from "../../../types/TableColumnHeader.ts";
import {Capability} from "../../../types/Capability.ts";

const CapabilityList = () => {
    const { data, status, isSuccess } = useFetchCapabilities();
    const deleteCapabilityMutation = useDeleteCapability();
    
    const tableHeaders: TableColumnHeader[] = [
        { label: "Id", accessor: "id", sortable: true },
        { label: "Name", accessor: "name", sortable: true },
        { label: "", accessor: "", sortable: false }
    ]
    
    const onDeleteClick = (item: Capability) => {
        if (window.confirm(`Are you sure you want to delete "${item.name}"?`)) {
            deleteCapabilityMutation.mutate(item);
        }
    }
    
    return(
        <>
            {!isSuccess && <ApiStatus status={status}/>}
            {isSuccess && 
                <Table<Capability> 
                    columnHeaders={tableHeaders} 
                    data={data} 
                    title="Capabilities"
                    caption="Manage capabilities"
                    onDelete={onDeleteClick}
                />
            }
        </>    
    );
}

export default CapabilityList;