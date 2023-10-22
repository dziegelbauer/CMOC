import ApiStatus from "../../../components/ApiStatus.tsx";
import Table from "../../../components/table/Table.tsx";
import {TableColumnHeader} from "../../../types/TableColumnHeader.ts";
import {Location} from "../../../types/Location.ts";
import {useDeleteLocation, useFetchLocations} from "../../../hooks/LocationHooks.ts";

const LocationList = () => {
    const { data, status, isSuccess } = useFetchLocations();
    const deleteLocationMutation = useDeleteLocation();
    
    const tableHeaders: TableColumnHeader[] = [
        { label: "Id", accessor: "id", sortable: true },
        { label: "Name", accessor: "name", sortable: true },
        { label: "", accessor: "", sortable: false }
    ]
    
    const onDeleteClick = (item: Location) => {
        if (window.confirm(`Are you sure you want to delete "${item.name}"?`))
            deleteLocationMutation.mutate(item);
    }
    
    return(
        <>
            {!isSuccess && <ApiStatus status={status}/>}
            {isSuccess && 
                <Table<Location> 
                    columnHeaders={tableHeaders} 
                    data={data} 
                    title="Locations"
                    caption="Manage locations"
                    onDelete={onDeleteClick}
                />
            }
        </>    
    );
}

export default LocationList;