import CapabilityForm from "./CapabilityForm.tsx";
import {Capability} from "../../../types/Capability.ts";
import {useAddCapability} from "../../../hooks/CapabilityHooks.ts";

const AddCapability = () => {
    const addCapabilityMutation = useAddCapability();
    
    const capability: Capability = {
        id: 0,
        name: "",
        dependencies: [],
        status: 0
    }
    
    const handleSubmit = (c: Capability) => {
        addCapabilityMutation.mutate(c);
    }
    
    return(
        <CapabilityForm capability={capability} callback={handleSubmit}/>
    )
}

export default AddCapability;