import axios, {AxiosError, AxiosResponse} from "axios";
import {Capability} from "../types/Capability.ts";
import {useMutation, useQuery, useQueryClient} from "react-query";
import Problem from "../types/Problem.ts";
import {useNavigate} from "react-router-dom";

const useFetchCapability = (id: number) => {
    return useQuery<Capability, AxiosError<Problem>>(["capabilities", id], () =>
        axios
            .get(`http://localhost:5222/api/v1/Capabilities/${id}`)
            .then(resp => resp.data.payload));
}

const useFetchCapabilities = () => {
    return useQuery<Capability[], AxiosError<Problem>>("capabilities", () =>
        axios
            .get(`http://localhost:5222/api/v1/Capabilities`)
            .then(resp => resp.data.payload));
}

const useAddCapability = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Capability>((c) =>
            axios.post(`http://localhost:5222/api/v1/Capabilities`, c), {
            onSuccess: () => {
                queryClient.invalidateQueries("capabilities");
                nav('/admin/capabilities');
            }
        }
    );
}

const useUpdateCapability = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError<Problem>, Capability>((c) =>
            axios.put(`http://localhost:5222/api/v1/Capabilities`, c), {
            onSuccess: (_, __) => {
                queryClient.invalidateQueries("capabilities");
                nav(`/admin/capabilities`);
            }
        }
    );
}

const useDeleteCapability = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, Capability>((c) =>
            axios.delete(`http://localhost:5222/api/v1/Capabilities/${c.id}`), {
            onSuccess: () => {
                queryClient.invalidateQueries("capabilities");
                nav('/admin/capabilities');
            }
        }
    );
}

export {
    useFetchCapability,
    useFetchCapabilities,
    useAddCapability,
    useDeleteCapability,
    useUpdateCapability
};