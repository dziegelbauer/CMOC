import {Issue} from "./Issue.ts";
import {Component} from "./Component.ts";

export type Equipment = {
    id: number;
    serialNumber: string;
    components: Component[];
    typeId: number;
    typeName: string;
    locationId: number;
    location: string;
    supportedServices: number[];
    notes: string | null;
    operationalOverride: boolean | null;
    issueId: number | null;
    issue: Issue | null;
    status: number;
}