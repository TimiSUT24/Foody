import {createContext, useContext, useState, useEffect} from 'react'
import { jwtDecode } from 'jwt-decode';
import {AuthService} from "../Services/AuthService"

const decodeUser = (token) => {
  try {
    const decoded = jwtDecode(token);

    if(decoded.exp * 1000 < Date.now()){
        return null;
    }
    return {
      id: decoded.sub || decoded.id,
      email:
        decoded.email ||
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
      username:
        decoded.username ||
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
        roles: decoded.roles ||
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
    };
  } catch {
    return null;
  }
};

const AuthContext = createContext();

export function AuthProvider({children}){
    const [accessToken, setAccessToken] = useState(() => {
        return localStorage.getItem("accessToken") || null
    });

    const [user, setUser] = useState(() =>
    accessToken ? decodeUser(accessToken) : null)

    useEffect(() => {
        const decoded = accessToken ? decodeUser(accessToken) : null;

        if(!decoded){
            setAccessToken(null);
            setUser(null);
        }else{
            setUser(decoded);
        }
}, [accessToken]);

    useEffect(() => {
        if(accessToken){
            localStorage.setItem("accessToken", accessToken);
        }else{
            localStorage.removeItem("accessToken");
        }
    }, [accessToken])

    //set new accessToken when updating
    const updateProfile = async (data) => {
        const response = await AuthService.updateProfile(data);

        if(response.accessToken){
            setAccessToken(response.accessToken);
            setUser(decodeUser(response.accessToken));
        }
        return response
    }

     const changePassword = async (data) => {
        const response = await AuthService.changePassword(data);

        if(response.accessToken){
            setAccessToken(response.accessToken);
            setUser(decodeUser(response.accessToken));
        }
        return response
    }


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
            refreshAccessToken,
            updateProfile,
            changePassword
        }}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => useContext(AuthContext); 