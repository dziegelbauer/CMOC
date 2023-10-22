import LocationForm from "./LocationForm.tsx";
import {useParams} from "react-router-dom";
import {Location} from "../../../types/Location.ts";
import {useFetchLocation, useUpdateLocation} from "../../../hooks/LocationHooks.ts";
import ApiStatus from "../../../components/ApiStatus.tsx";

const EditLocation = () => {
    const { id } = useParams();
    if (!id) throw Error("Location not found");
    const locationId = parseInt(id);

    const { data, status, isSuccess } = useFetchLocation(locationId);
    const updateLocationMutation = useUpdateLocation();

    if (!isSuccess) return <ApiStatus status={status}/>;
    if (!data) return <div>Capability not found</div>;
    
    const handleSubmit = (l: Location) => {
        updateLocationMutation.mutate(l);
    }
    
    return (
        <LocationForm location={data} callback={handleSubmit}/>
    )
}

export default EditLocation;