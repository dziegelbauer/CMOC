import {useAddService} from "../../../hooks/ServiceHooks.ts";
import ServiceForm from "./ServiceForm.tsx";
import {Service} from "../../../types/Service.ts";

const AddService = () => {
    const addServiceMutation = useAddService();

    const newService: Service = {
        id: 0,
        name: "",
        status: 0,
        dependents: [],
        dependencies: []
    };
    
    const handleSubmit = (c: Service) => {
        addServiceMutation.mutate(c);
    }

    return (
        <ServiceForm service={newService} callback={handleSubmit}/>
    )
}

export default AddService;