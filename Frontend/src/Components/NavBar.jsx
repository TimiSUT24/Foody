import {NavLink, useLocation} from 'react-router-dom'
import {useEffect, useState} from 'react'
import {useCart} from "../Context/CartContext"
import "../CSS/NavBar.css"


export default function NavBar (){
    //const {user} = useAuth();
    const [menuOpen, setMenuOpen] = useState(false);
    const location = useLocation();
    const {totalItems, totalPrice} = useCart();

    const toggleMenu = () => setMenuOpen(p => !p)

    //const roles = Array.isArray(user?.roles) ? user.roles : user?.role ? [user.role] : [];

    //const isAdmin = roles.includes("Admin");
    //const isUser = roles.includes("User");

    useEffect(() => {
      setMenuOpen(false);
    },[location.pathname])

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
    /*if (isAdmin) {
      return (
        <>
          <NavLink to="/">Hem</NavLink>
          <NavLink to="/products">Produkter</NavLink>
          <NavLink to="/cart">Varukorg</NavLink>
          <NavLink to="/admin">Admin</NavLink>
          <NavLink to="/profile">Min Profil</NavLink>
        </>
      );
    }*/

    //LOGGED IN USER
    /*if (isUser) {
      return (
        <>
          <NavLink to="/">Hem</NavLink>
          <NavLink to="/products">Produkter</NavLink>
          <NavLink to="/cart">Varukorg</NavLink>
          <NavLink to="/profile">Min Profil</NavLink>
        </>
      );
    }*/

    //LOGGED OUT 
    return (
      <>
        <NavLink to="/">Hem</NavLink>
        <NavLink to="/deals">Erbjudanden</NavLink>
        <NavLink to="/about">Om</NavLink>
        <NavLink to="/login">Logga in</NavLink>
        <NavLink to="/register">Registrera</NavLink>
        <NavLink to="/cart">Varukorg {totalPrice.toFixed(2)} kr <p id="navbar-quantity" >{totalItems !== 0 && totalItems}</p></NavLink>   
        <NavLink to="/user-page">Mina sidor</NavLink>
        <NavLink to="/thank-you-page">thankyou</NavLink>
        
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