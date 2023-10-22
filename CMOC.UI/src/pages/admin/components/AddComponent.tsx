import {Component} from "../../../types/Component.ts";
import ComponentForm from "./ComponentForm.tsx";
import {useAddComponent} from "../../../hooks/ComponentHooks.ts";

const AddComponent = () => {
    const addComponentMutation = useAddComponent();

    const component: Component = {
        id: 0,
        equipmentId: 0,
        operational: true,
        componentOfId: 0,
        typeName: "",
        equipment: "",
        issueDto: null,
        issueId: null,
        serialNumber: "",
        typeId: 0,
        status: 0
    }

    const handleSubmit = (c: Component) => {
        addComponentMutation.mutate(c);
    }

    return(
        <ComponentForm component={component} callback={handleSubmit}/>
    )
}

export default AddComponent;