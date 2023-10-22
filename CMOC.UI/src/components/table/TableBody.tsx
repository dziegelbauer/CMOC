import {TableColumnHeader} from "../../types/TableColumnHeader.ts";
import {IDataItem} from "../../types/IDataItem.ts";
import {Link} from "react-router-dom";
import {PencilSquare, TrashFill} from "react-bootstrap-icons";

type Args<T extends IDataItem> = {
    columns: TableColumnHeader[];
    data: T[];
    onDelete: (item: T) => void;
}

function TableBody<T extends IDataItem>({columns, data, onDelete}: Args<T>) {
    return (
        <tbody>
        {data.map(row => {
            return (
                <tr key={row.id}>
                    {columns.map(({accessor}) => {
                        if(accessor === "") {
                            return (
                                <td key={accessor}>
                                    <div className="btn-group" role="group">
                                        <Link to={`${row.id}`} className="btn btn-primary">
                                            <PencilSquare/>
                                        </Link>
                                        <button className="btn btn-danger" onClick={() => onDelete(row)}>
                                            <TrashFill/>
                                        </button>
                                    </div>
                                </td>
                            );
                        } else {
                            return <td key={accessor}>{row[accessor]}</td>;
                        }
                    })}
                </tr>
            )
        })}
        </tbody>
    )
}

export default TableBody;