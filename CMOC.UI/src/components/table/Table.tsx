import TableHead from "./TableHead.tsx";
import TableBody from "./TableBody.tsx";
import {TableColumnHeader} from "../../types/TableColumnHeader.ts";
import {IDataItem} from "../../types/IDataItem.ts";
import classes from "./Table.module.css";
import {useState} from "react";
import {Link} from "react-router-dom";
import {PlusSquare} from "react-bootstrap-icons";

type Args<T extends IDataItem> = {
    columnHeaders: TableColumnHeader[];
    data: T[];
    title: string;
    caption: string;
    onDelete: (item: T) => void;
}

function Table<T extends IDataItem>({columnHeaders, data, title, caption, onDelete}: Args<T>) {
    const [tableData, setTableData] = useState(data);
    
    const handleSorting = (sortField: string, sortOrder: string) => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortField] === null) return 1;
                if (b[sortField] === null) return -1;
                if (a[sortField] === null && b[sortField] === null) return 0;
                return (
                    a[sortField].toString().localeCompare(b[sortField].toString(), "en", {
                        numeric: true,
                    }) * (sortOrder === "asc" ? 1 : -1)
                );
            });
            setTableData(sorted);
        }
    }
    
    return (
        <div className={classes.table_container}>
            <div className="row">
                <div className="col-6">
                    <h1>{title}</h1>
                </div>
                <div className="col-6 text-end">
                    <Link to="new " className="btn btn-primary">
                        <PlusSquare className="mb-1"/> Create New
                    </Link>
                </div>
            </div>
            <table className="table table-bordered table-striped">
                <caption>{caption}</caption>
                <TableHead columnHeaders={columnHeaders} handleSorting={handleSorting}/>
                <TableBody data={tableData} columns={columnHeaders} onDelete={onDelete}/>
            </table>
        </div>
    )
}

export default Table;