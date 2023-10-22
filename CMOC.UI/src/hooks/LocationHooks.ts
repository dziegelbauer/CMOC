import axios, {AxiosError, AxiosResponse} from "axios";
import {Location} from "../types/Location.ts";
import {useMutation, useQuery, useQueryClient} from "react-query";
import Problem from "../types/Problem.ts";
import {useNavigate} from "react-router-dom";

const useFetchLocation = (id: number) => {
    return useQuery<Location, AxiosError<Problem>>(["locations", id], () =>
        axios
            .get(`http://localhost:5222/api/v1/Locations/${id}`)
            .then(resp => resp.data.payload));
}

const useFetchLocations = () => {
    return useQuery<Location[], AxiosError<Problem>>("locations", () =>
        axios
            .get(`http://localhost:5222/api/v1/Locations`)
            .then(resp => resp.data.payload));
}

const useAddLocation = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Location>((c) =>
            axios.post(`http://localhost:5222/api/v1/Locations`, c), {
            onSuccess: () => {
                queryClient.invalidateQueries("locations");
                nav('/admin/locations');
            }
        }
    );
}

const useUpdateLocation = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Location>((c) =>
            axios.put(`http://localhost:5222/api/v1/Locations`, c), {
            onSuccess: (_, __) => {
                queryClient.invalidateQueries("locations");
                nav(`/admin/locations`);
            }
        }
    );
}

const useDeleteLocation = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, Location>((l) =>
            axios.delete(`http://localhost:5222/api/v1/Locations/${l.id}`), {
            onSuccess: () => {
                queryClient.invalidateQueries("locations");
                nav('/admin/locations')
            }
        }
    );
}

export {
    useFetchLocation,
    useFetchLocations,
    useAddLocation,
    useUpdateLocation,
    useDeleteLocation
};