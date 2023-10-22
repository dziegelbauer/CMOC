import './App.css'
import Header from "./layout/Header.tsx";
import {Route, Routes} from "react-router-dom";
import Status from "./pages/Status.tsx";
import CapabilityList from "./pages/admin/capabilities/CapabilityList.tsx";
import ServiceList from "./pages/admin/services/ServiceList.tsx";
import EquipmentList from "./pages/admin/equipment/EquipmentList.tsx";
import ComponentList from "./pages/admin/components/ComponentList.tsx";
import LocationList from "./pages/admin/locations/LocationList.tsx";
import AddCapability from "./pages/admin/capabilities/AddCapability.tsx";
import EditCapability from "./pages/admin/capabilities/EditCapability.tsx";
import EditService from "./pages/admin/services/EditService.tsx";
import AddService from "./pages/admin/services/AddService.tsx";
import AddComponent from "./pages/admin/components/AddComponent.tsx";
import EditComponent from "./pages/admin/components/EditComponent.tsx";
import AddEquipment from "./pages/admin/equipment/AddEquipment.tsx";
import AddLocation from "./pages/admin/locations/AddLocation.tsx";
import EditEquipment from "./pages/admin/equipment/EditEquipment.tsx";
import EditLocation from "./pages/admin/locations/EditLocation.tsx";

function App() {
    return (
        <>
            <Header/>
            <div className="container">
                <main role="main" className="pb-3">
                    <Routes>
                        <Route path="/" element={<Status tier="capability"/>}/>
                        <Route path="/status" element={<Status tier="capability"/>}/>
                        <Route path="/status/capabilities" element={<Status tier="capability"/>}/>
                        <Route path="/status/services" element={<Status tier="service"/>}/>
                        <Route path="/status/services/supporting/:id" element={<Status tier="service"/>}/>
                        <Route path="/status/equipment" element={<Status tier="equipment"/>}/>
                        <Route path="/status/equipment/supporting/:id" element={<Status tier="equipment"/>}/>
                        <Route path="/admin/capabilities" element={<CapabilityList/>}/>
                        <Route path="/admin/capabilities/new" element={<AddCapability/>}/>
                        <Route path="/admin/capabilities/:id" element={<EditCapability/>}/>
                        <Route path="/admin/services" element={<ServiceList/>}/>
                        <Route path="/admin/services/new" element={<AddService/>}/>
                        <Route path="/admin/services/:id" element={<EditService/>}/>
                        <Route path="/admin/equipment" element={<EquipmentList/>}/>
                        <Route path="/admin/equipment/new" element={<AddEquipment/>}/>
                        <Route path="/admin/equipment/:id" element={<EditEquipment/>}/>
                        <Route path="/admin/components" element={<ComponentList/>}/>
                        <Route path="/admin/components/new" element={<AddComponent/>}/>
                        <Route path="/admin/components/:id" element={<EditComponent/>}/>
                        <Route path="/admin/locations" element={<LocationList/>}/>
                        <Route path="/admin/locations/new" element={<AddLocation/>}/>
                        <Route path="/admin/locations/:id" element={<EditLocation/>}/>
                    </Routes>
                </main>
            </div>
        </>
    )
}

export default App
