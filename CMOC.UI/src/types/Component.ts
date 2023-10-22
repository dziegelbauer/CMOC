import {Issue} from "./Issue.ts";

export type Component = {
    id: number;
    serialNumber: string;
    typeId: number;
    typeName: string;
    operational: boolean;
    componentOfId: number;
    equipmentId?: number;
    equipment?: string;
    issueId: number | null;
    issue: Issue | null;
    status: number;
}