import {Navigate, Outlet} from 'react-router-dom'
import {useAuth} from '../Context/AuthContext'

export default function UserProtectedRoute({roles}){
    const {user} = useAuth();

    if(!user){
        return <Navigate to="/login" replace />
    }

    if(roles && !roles.some(s => user.roles?.includes(s))){
        return <Navigate to="/" replace />
    }

    return <Outlet />;
}