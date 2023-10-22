import {useFetchService, useUpdateService} from "../../../hooks/ServiceHooks.ts";
import {useParams} from "react-router-dom";
import ApiStatus from "../../../components/ApiStatus.tsx";
import ServiceForm from "./ServiceForm.tsx";
import {Service} from "../../../types/Service.ts";

const EditService = () => {
    const { id } = useParams();
    if (!id) throw Error("Service not found");
    const ServiceId = parseInt(id);

    const { data, status, isSuccess } = useFetchService(ServiceId);
    const updateServiceMutation = useUpdateService();

    if (!isSuccess) return <ApiStatus status={status}/>;
    if (!data) return <div>Service not found</div>;

    const handleSubmit = (c: Service) => {
        updateServiceMutation.mutate(c);
    }

    return (
        <ServiceForm service={data} callback={handleSubmit}/>
    )
}

export default EditService;