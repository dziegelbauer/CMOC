import {Location} from "../../../types/Location.ts";
import {useState} from "react";
import {Link} from "react-router-dom";

type Args = {
    location: Location;
    callback: (location: Location) => void;
}

const LocationForm = ({location, callback}: Args) => {
    const [formLocation, setFormLocation] = useState(location);

    return (
        <form onSubmit={(e) => {
            e.preventDefault();
            callback(formLocation);
        }}>
            <input defaultValue={location.id} hidden/>
            <div className="border p-3 mt-4">
                <div className="row pb-2">
                    <div className="col-9">
                        <h2 className="pl-3">
                            {location.id == 0 ? "Create" : "Update"} Location
                        </h2>
                        <hr/>

                        <div className="mb-3 form-floating">
                            <input
                                type="text"
                                id="capabilityName"
                                value={formLocation.name}
                                className="form-control"
                                placeholder="Name"
                                onChange={(e) =>
                                    setFormLocation({...formLocation, name: e.target.value})}
                            />
                            <label htmlFor="capabilityName" className="form-label">Name</label>
                        </div>
                    </div>
                </div>
                {location.id === 0 &&
                    <button type="submit" className="btn btn-primary mx-1">Create</button>
                }
                {location.id !== 0 &&
                    <button type="submit" className="btn btn-primary mx-1">Update</button>
                }
                <Link to="/admin/locations" className="btn btn-secondary">Back to List</Link>
            </div>
        </form>
    )
}

export default LocationForm;