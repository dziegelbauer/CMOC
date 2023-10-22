import ApiStatus from "../../../components/ApiStatus.tsx";
import Table from "../../../components/table/Table.tsx";
import {TableColumnHeader} from "../../../types/TableColumnHeader.ts";
import {Service} from "../../../types/Service.ts";
import {useDeleteService, useFetchServices} from "../../../hooks/ServiceHooks.ts";

const ServiceList = () => {
    const { data, status, isSuccess } = useFetchServices();
    const deleteServiceMutation = useDeleteService();

    const tableHeaders: TableColumnHeader[] = [
        { label: "Id", accessor: "id", sortable: true },
        { label: "Name", accessor: "name", sortable: true },
        { label: "", accessor: "", sortable: false }
    ];
    
    const onDeleteClick = (item: Service) => {
        if (window.confirm(`Are you sure you want to delete "${item.name}"?`))
            deleteServiceMutation.mutate(item);
    }

    return(
        <>
            {!isSuccess && <ApiStatus status={status}/>}
            {isSuccess &&
                <Table<Service>
                    columnHeaders={tableHeaders}
                    data={data}
                    title="Services"
                    caption="Manage services"
                    onDelete={onDeleteClick}
                />
            }
        </>
    );
}

export default ServiceList;