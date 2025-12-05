import {createContext, useContext, useState, useEffect} from 'react'
import {AuthService} from "../Services/AuthService"

const AuthContext = createContext();

export function AuthProvider({children}){
    const [accessToken, setAccessToken] = useState(() => {
        return localStorage.getItem("accessToken") || null
    });

    const [user, setUser] = useState(null);

    useEffect(() => {
        if(accessToken){
            localStorage.setItem("accessToken", accessToken);
        }else{
            localStorage.removeItem("accessToken");
        }
    }, [accessToken])

    const login = async (email, password) => {
        const data = await AuthService.login(email,password);
        setAccessToken(data.accessToken);
        return true;
    };

    const logout = async () => {
        await AuthService.logout();
        setAccessToken(null);
        setUser(null);
    }

    const refreshAccessToken = async () => {
        try{
            const data = await AuthService.refresh();
            setAccessToken(data.accessToken);
            return data.accessToken;

        }catch{
            logout();
            return null;
        }
    }

    return (
        <AuthContext.Provider value ={{
            accessToken,
            user,
            login,
            logout,
            refreshAccessToken
        }}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => useContext(AuthContext); 