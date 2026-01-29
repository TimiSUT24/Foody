import {NavLink, useLocation, useNavigate} from 'react-router-dom'
import {useEffect, useState} from 'react'
import {useCart} from "../Context/CartContext"
import api from "../Api/api"
import { PiShoppingCartSimpleLight } from "react-icons/pi";
import { useAuth } from "../Context/AuthContext";
import "../CSS/NavBar.css"


export default function NavBar (){
    const {user} = useAuth();
    const [menuOpen, setMenuOpen] = useState(false);
    const [totalPrice, setTotalPrice] = useState(0);
    const location = useLocation();
    const {totalItems, cart} = useCart();
    const navigate = useNavigate();

    const toggleMenu = () => setMenuOpen(p => !p)

    const roles = user?.roles ?? [];

    const isAdmin = roles.includes("Admin");
    const isUser = roles.includes("User");

    //handle so user cant click cart link if logged out
    const handleCartLink = (e) => {
      if(!user){
        e.preventDefault();
        navigate("/login");
      }
    }
    
    useEffect(() => {
      setMenuOpen(false);
    },[location.pathname])

      useEffect(() => {
    const fetchTotal = async () => {
      if(cart.length <= 0){
            setTotalPrice(0);
            return;
        }
        const response = await api.post("/api/Order/CalculateTax", {items: cart, serviceCode:""})
        setTotalPrice(response.data.total);
    };
    fetchTotal();
   

  },[cart])
    
    useEffect(() => {
    // Map paths to body CSS classes
    const bodyClassMap = {
      "/": "home-body",
      "/products": "products-body",
      "/product": "product-body",
      "/cart": "cart-body",
      "/login": "login-body",
      "/register": "register-body",
      "/profile": "profile-body",
      "/admin": "admin-body",
      "/order-complete": "orderComplete-body",
      "/user-page": "user-body"
    };

    // Default class
    let newClass = bodyClassMap[location.pathname] || "default-body";

    if (location.pathname.startsWith("/product/")) {
      newClass = "product-body";
    }

    const allClasses = Object.values(bodyClassMap).concat("default-body");
    allClasses.forEach(c => document.body.classList.remove(c));

    document.body.classList.add(newClass);

    return () => document.body.classList.remove(newClass);
  }, [location]);

    const renderLinks = () => {
    //ADMIN
    if (isAdmin) {
      return (
        <>
          <NavLink to="/">Hem</NavLink>
          <NavLink to="/cart">Varukorg</NavLink>
          <NavLink to="/admin">Admin</NavLink>
          <NavLink to="/profile">Min Profil</NavLink>
        </>
      );
    }

    //LOGGED IN USER
    if (isUser) {
      return (
        <>
          <NavLink to="/">Hem</NavLink>
          <NavLink to="/about">Om</NavLink>
          <NavLink className="cart-link" to="/cart"><PiShoppingCartSimpleLight style={{width:23,height:23,color:"white",marginRight:7}} />Varukorg {totalPrice.toFixed(2)} kr
            {totalItems !== 0 &&  <div className="quantity-div">
                  <p id="navbar-quantity" >{totalItems !== 0 && totalItems}</p>
                </div>}       
          </NavLink>
          <NavLink to="/user-page">Mina sidor</NavLink>
        </>
      );
    }

    //LOGGED OUT 
    return (
      <>
        <NavLink to="/">Hem</NavLink>
        <NavLink to="/about">Om</NavLink>
        <NavLink to="/login">Logga in</NavLink>
        <NavLink to="/register">Registrera</NavLink>      
        <NavLink className={`cart-link ${!user ? "disabled" : ""}`} to="/cart" onClick={handleCartLink}><PiShoppingCartSimpleLight style={{width:23,height:23,color:"white",marginRight:7}} />Varukorg {totalPrice.toFixed(2)} kr
        {totalItems !== 0 &&  <div className="quantity-div">
              <p id="navbar-quantity" >{totalItems !== 0 && totalItems}</p>
            </div>}       
        </NavLink>         
      </> 
    );
  };

    return (
        <nav className="navbar">
      <div className="navbar-left">
        <NavLink className="navbar-logo" to="/">
          <h1>Foody</h1>
        </NavLink>
      </div>     

      <div className={`navbar-links ${menuOpen ? "open" : ""}`}>
        {renderLinks()}
      </div>
       <div className="hamburger" onClick={toggleMenu}>
      <span />
      <span />
      <span />
    </div>
    </nav>
    )
}