import {useFetchCapability, useUpdateCapability} from "../../../hooks/CapabilityHooks.ts";
import {useParams} from "react-router-dom";
import ApiStatus from "../../../components/ApiStatus.tsx";
import CapabilityForm from "./CapabilityForm.tsx";
import {Capability} from "../../../types/Capability.ts";

const EditCapability = () => {
    const { id } = useParams();
    if (!id) throw Error("Capability not found");
    const capabilityId = parseInt(id);
    
    const { data, status, isSuccess } = useFetchCapability(capabilityId);
    const updateCapabilityMutation = useUpdateCapability();

    if (!isSuccess) return <ApiStatus status={status}/>;
    if (!data) return <div>Capability not found</div>;

    const handleSubmit = (c: Capability) => {
        updateCapabilityMutation.mutate(c);
    }
    
    return (
        <CapabilityForm capability={data} callback={handleSubmit}/>
    )
}

export default EditCapability;