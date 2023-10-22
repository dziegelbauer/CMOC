import {Capability} from "../../../types/Capability.ts";
import {useState} from "react";
import {Link} from "react-router-dom";

type Args = {
    capability: Capability;
    callback: (capability: Capability) => void;
}

const CapabilityForm = ({capability, callback}: Args) => {
    const [formCapability, setFormCapability] = useState(capability);
    
    return (
        <form onSubmit={(e) => {
            e.preventDefault();
            callback(formCapability);
        }}>
            <input defaultValue={capability.id} hidden/>
            <div className="border p-3 mt-4">
                <div className="row pb-2">
                    <div className="col-9">
                        <h2 className="pl-3">
                            {capability.id == 0 ? "Create" : "Update"} Capability
                        </h2>
                        <hr/>

                        <div className="mb-3 form-floating">
                            <input 
                                type="text"
                                id="capabilityName"
                                value={formCapability.name} 
                                className="form-control"
                                placeholder="Name"
                                onChange={(e) => 
                                    setFormCapability({...formCapability, name: e.target.value})}
                            />
                            <label htmlFor="capabilityName" className="form-label">Name</label>
                        </div>
                    </div>
                </div>
                {capability.id === 0 &&
                    <button type="submit" className="btn btn-primary mx-1">Create</button>
                }
                {capability.id !== 0 &&
                    <button type="submit" className="btn btn-primary mx-1">Update</button>
                }
                <Link to="/admin/capabilities" className="btn btn-secondary">Back to List</Link>
            </div>
        </form>
    )
}

export default CapabilityForm;