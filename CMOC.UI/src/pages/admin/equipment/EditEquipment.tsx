import EquipmentForm from "./EquipmentForm.tsx";
import {useParams} from "react-router-dom";
import {useFetchEquipmentItem, useUpdateEquipment} from "../../../hooks/EquipmentHooks.ts";
import ApiStatus from "../../../components/ApiStatus.tsx";
import {Equipment} from "../../../types/Equipment.ts";

const EditEquipment = () => {
    const { id } = useParams();
    if (!id) throw Error("Equipment item not found");
    const equipmentId = parseInt(id);

    const { data, status, isSuccess } = useFetchEquipmentItem(equipmentId);
    const updateEquipmentMutation = useUpdateEquipment();

    if (!isSuccess) return <ApiStatus status={status}/>;
    if (!data) return <div>Equipment item not found</div>;
    
    const handleSubmit = (e: Equipment) => {
        updateEquipmentMutation.mutate(e);
    }
    
    return (
        <EquipmentForm equipment={data} callback={handleSubmit}/>
    )
}

export default EditEquipment;