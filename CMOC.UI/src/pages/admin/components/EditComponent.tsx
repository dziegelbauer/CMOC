import {useParams} from "react-router-dom";
import ComponentForm from "./ComponentForm.tsx";
import {useFetchComponent, useUpdateComponent} from "../../../hooks/ComponentHooks.ts";
import {Component} from "../../../types/Component.ts";
import ApiStatus from "../../../components/ApiStatus.tsx";

const EditComponent = () => {
    const { id } = useParams();
    if (!id) throw Error("Capability not found");
    const componentId = parseInt(id);

    const { data, status, isSuccess } = useFetchComponent(componentId);
    const updateComponentMutation = useUpdateComponent();

    if (!isSuccess) return <ApiStatus status={status}/>;
    if (!data) return <div>Component not found</div>;
    
    const handleSubmit = (c: Component) => {
        updateComponentMutation.mutate(c);
    }
    
    return (
        <ComponentForm component={data} callback={handleSubmit}/>
    )
}

export default EditComponent;