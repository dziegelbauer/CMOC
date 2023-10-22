import {useAddLocation} from "../../../hooks/LocationHooks.ts";
import {Location} from "../../../types/Location.ts"
import LocationForm from "./LocationForm.tsx"; 

const AddLocation = () => {
    const addLocatgionMutation = useAddLocation();
    
    const location: Location = {
        id: 0,
        name: ""
    }
    
    const handleSubmit = (l: Location) => {
        addLocatgionMutation.mutate(l);
    }
    
    return (
        <LocationForm location={location} callback={handleSubmit}/>
    )
}

export default AddLocation;