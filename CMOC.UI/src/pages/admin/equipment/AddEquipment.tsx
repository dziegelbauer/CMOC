import {useAddEquipment} from "../../../hooks/EquipmentHooks.ts";
import {Equipment} from "../../../types/Equipment.ts";
import EquipmentForm from "./EquipmentForm.tsx";

const AddEquipment = () => {
    const addEquipmentMutation = useAddEquipment();
    
    const equipment: Equipment = {
        id: 0,
        notes: "",
        locationId: 0,
        typeId: 0,
        status: 0,
        serialNumber: "",
        supportedServices: [],
        components: [],
        issue: null,
        issueId: null,
        location: "",
        operationalOverride: null,
        typeName: ""
    }
    
    const handleSubmit = (e: Equipment) => {
        addEquipmentMutation.mutate(e);
    }
    
    return (
        <EquipmentForm equipment={equipment} callback={handleSubmit}/>
    )
}

export default AddEquipment;