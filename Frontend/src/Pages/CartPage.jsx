import {useCart} from "../Context/CartContext"
import {useState,useEffect} from "react"
import api from "../Api/api"
import { createPaymentIntent } from "../Services/StripeService"
import { loadStripe } from "@stripe/stripe-js"
import { Elements } from "@stripe/react-stripe-js"
import CheckoutForm from "../Components/CheckoutForm"
import { getDeliveryOptions } from "../Services/PostnordService"
import { useAuth } from "../Context/AuthContext"
import { CiCirclePlus } from "react-icons/ci";
import { CiCircleMinus } from "react-icons/ci";
import "../CSS/CartPage.css"

const stripePromise = loadStripe(import.meta.env.VITE_STRIPE_PUBLISH_KEY)

export default function CartPage(){
    const {cart, addToCart, removeFromCart} = useCart();
    const [error, setError] = useState({})
    const [clientSecret, setClientSecret] = useState(null);
    const [userId, setUserId] = useState(null)
    const {user} = useAuth();
    //postnord
    const [deliver, setDeliver] = useState(false);
    const [postnordOptions, setPostnordOptions] = useState(null);
    const [selectedOption, setSelectedOption] = useState(null);
    //Taxes
    const [totals, setTotal] = useState({
        subTotal: 0,
        moms: 0,
        total:0,
        shippingTax: 0,
    })
    const [shippingTax, setShippingTax] = useState(0);

     const appearance = {
    theme: 'stripe',
  };
  // Enable the skeleton loader UI for optimal loading.
  const loader = 'auto';

  useEffect(() => {
    const fetchTotal = async () => {
        const response = await api.post("/api/Order/CalculateTax", {items: cart, serviceCode:shipping.serviceCode})
        setTotal(response.data)
        setUserId(user.id)
    };
    fetchTotal();
   

  },[cart])
    
  //Shipping logic
    const [shipping, setShipping] = useState({
        firstName:"",
        lastName:"",
        email:"",
        adress:"",
        city:"",
        state:"",
        postalCode:"",
        phoneNumber:"",
        deliveryOptionId:"",
        serviceCode:""
    });

    const handleDeliverySelect = (option) => {
        setSelectedOption(option);
        setShipping(prev => ({
            ...prev,
            deliveryOptionId: option.deliveryOptionId,
            serviceCode: option.serviceCode
        }))
    }

    useEffect(() => {
        if(!shipping.serviceCode) return;

        const recalcTotals = async () => {
            const response = await api.post("/api/Order/CalculateTax", {items: cart, serviceCode:shipping.serviceCode})
            setTotal(response.data);
            setShippingTax(response.data.shippingTax)
        }
        recalcTotals();
    },[shipping.serviceCode])

     const handleChange = (e) => {
        setShipping({...shipping, [e.target.name]: e.target.value})
        setError(error => ({...error, [e.target.name]: ""}));
    }

    //Place Order
    const placeOrder = async () => {
        const required = ["firstName","lastName","email","adress","city","state","postalCode","phoneNumber"];
        const newErrors = {};

        required.forEach(field => {
            if(!shipping[field] || shipping[field].trim() === ""){
                newErrors[field] = "This field is required";
            }
                });
            setError(newErrors);

            if(Object.keys(newErrors).length > 0){
                return;
            }

            if(!selectedOption){
                alert("Choose delivery option")
                return;
            }

         try {
        const {clientSecret} = await createPaymentIntent(
        cart.map(i => ({
          price: i.price,
          qty: i.qty,
          name: i.name
        })),
        shipping,
        shippingTax,
        userId
      );

      setClientSecret(clientSecret);

    } catch(err) {
        console.error(err);
        alert("Klarna betalning misslyckades");
    }
    }

    const delivery = async () => {
        try{
            const required = ["firstName","lastName","email","adress","city","state","postalCode","phoneNumber"];
        const newErrors = {};

        required.forEach(field => {
            if(!shipping[field] || shipping[field].trim() === ""){
                newErrors[field] = "This field is required";
            }
                });
            setError(newErrors);

            if(Object.keys(newErrors).length > 0){
                return;
            }
            
            const options = await getDeliveryOptions({
            postCode: shipping.postalCode
        })
        if(options.status = 200){
            setPostnordOptions(options)
            setDeliver(true);
        }

        }catch(err){
            console.error(err);
        }
    }

    return(
        <div className="cart-page">
            <h1 id="cart-page-h3"style={{display:"flex",gridArea:"h1"}}>Varukorg</h1>

            <div id="shipping-information" style={{display:"flex",flexDirection:"column",gap:"10px",padding:"20px"}}>
                <h2 style={{textAlign:"left"}}>LeveransInformation</h2>

                <div className="shipping-info-combined" style={{display:"flex", justifyContent:"space-between"}}>

                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p className="shipping-info-p" style={{margin:"0",color: error.firstName ? "red" : "",}}>Förnamn <p style={{color:"red",margin:0}}>*</p></p>
                        <input className="shipping-info-inputs" type="text" name="firstName" value={shipping.firstName} onChange={handleChange} placeholder="Jan" style={{paddingLeft:"10px",border: error.firstName ? "2px solid red" : "",}} />
                    </div>

                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p className="shipping-info-p" style={{margin:"0",color: error.lastName ? "red" : ""}}>Efternamn <p style={{color:"red",margin:0}}>*</p></p>
                        <input className="shipping-info-inputs" type="text" name="lastName" value={shipping.lastName} onChange={handleChange} placeholder="Carlsson" style={{paddingLeft:"10px",border: error.lastName ? "2px solid red" : ""}} />
                    </div>              
                </div>

                <div style={{display:"flex", flexDirection:"column",gap:"20px"}}>

                    <div style={{display:"flex", flexDirection:"column", gap:"5px"}}>
                        <p className="shipping-info-p" style={{margin:"0",textAlign:"left",color: error.email ? "red" : ""}}>E-post <p style={{color:"red",margin:0}}>*</p></p>
                        <input type="email" name="email" value={shipping.email} onChange={handleChange} placeholder="jan@example.com" style={{paddingLeft:"10px",border: error.email ? "2px solid red" : ""}} />
                    </div>

                    <div style={{display:"flex", flexDirection:"column",gap:"5px"}}>
                        <p className="shipping-info-p" style={{margin:"0",textAlign:"left",color: error.adress ? "red" : ""}}>Gatuadress <p style={{color:"red",margin:0}}>*</p></p>
                        <input type="text" name="adress" value={shipping.adress} onChange={handleChange} placeholder="123 Malmgatan" style={{paddingLeft:"10px",border: error.adress ? "2px solid red" : ""}}/>
                    </div>              
                </div>
       
                <div className="shipping-info-combined" style={{display:"flex", justifyContent:"space-between"}}>

                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p className="shipping-info-p" style={{margin:"0",color: error.state ? "red" : ""}}>Län <p style={{color:"red",margin:0}}>*</p></p>
                        <input className="shipping-info-inputs" type="text" name="state" value={shipping.state} onChange={handleChange} placeholder="Halland" style={{paddingLeft:"10px",border: error.state ? "2px solid red" : ""}}/>
                    </div>   

                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p className="shipping-info-p" style={{margin:"0",color: error.phoneNumber ? "red" : ""}}>Telefonnummer <p style={{color:"red",margin:0}}>*</p></p>
                        <input className="postal-input" type="number" value={shipping.phoneNumber} onChange={handleChange} name="phoneNumber" placeholder="0721223333" style={{paddingLeft:"10px",border: error.phoneNumber ? "2px solid red" : ""}}/>
                    </div>                 
                </div>

                <div className="shipping-info-combined" style={{display:"flex", justifyContent:"space-between"}}>

                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p className="shipping-info-p"  style={{margin:"0",color: error.city ? "red" : ""}}>Ort <p style={{color:"red",margin:0}}>*</p></p>
                        <input className="shipping-info-inputs" type="text" name="city" value={shipping.city} onChange={handleChange} placeholder="Stockholm" style={{paddingLeft:"10px",border: error.city ? "2px solid red" : ""}}/>
                    </div>  
                                   
                    <div style={{display:"flex",flexDirection:"column",textAlign:"left",gap:"5px"}}>
                        <p className="shipping-info-p"  style={{margin:"0",color: error.postalCode ? "red" : ""}}>Postnummer <p style={{color:"red",margin:0}}>*</p></p>
                        <input className="postal-input"  type="number" value={shipping.postalCode} onChange={handleChange} name="postalCode" placeholder="10011" style={{paddingLeft:"10px",border: error.postalCode ? "2px solid red" : ""}}/>
                    </div>                 
                </div>
                 <button className="checkout-btn" onClick={delivery}>Fortsätt</button>
            </div>

            <div id="cart-container">
              
            {cart.length === 0 ? (
                <p>Din varukorg är tom</p>
            ) : (
                <>
                <div id="cart-header">
                    <h3>Order</h3>
                </div>

                <div className="cart-items">
                {cart.map(item => (
                    <div key={item.id} className="cart-item">                  
                        <img src={item.imageUrl} alt="" style={{width:50}}/>
                        <p className="p2" style={{fontSize:13}}>{item.name}</p>
                        <p style={{fontSize:13}}>{item.price} kr</p>

                        <div className="qty-controls">
                            <button onClick ={() => removeFromCart(item.id)} style={{borderStyle:"none",backgroundColor:"transparent"}}><CiCircleMinus className="ciCircleMinus"style={{width:22,height:22}}/></button>
                            <span>{item.qty}</span>
                            <button onClick ={() => addToCart(item)} style={{borderStyle:"none",backgroundColor:"transparent"}}><CiCirclePlus className="ciCirclePlus" style={{width:22,height:22}}/></button>                
                        </div>
                    </div>
                ))}
                </div>

                <div className="custom-hr"></div>

                <div className="checkout-prices">
                    
                    <div className="checkout-price">
                    <span>Subtotal:</span>
                    <span >{totals.subTotal} kr</span>
                    </div>

                    
                    <div className="checkout-price">
                    <span>Frakt:</span>
                    <span>{totals.shippingTax} kr</span>
                    </div>

                    <div className="checkout-price">
                    <span>Moms:</span>
                    <span>{totals.moms} kr</span>
                    </div>
                </div>

                <div className="custom-hr"></div>
                
                <h2>Totalt: {totals.total} kr</h2>

                {selectedOption && (
                    <button className="checkout-btn" onClick={placeOrder}>Lägg Order</button>
                )}
                </>
                
            )}
            </div>

            {deliver && (
                <div id="delivery">
                    <h2 style={{ textAlign: "left" }}>Välj leverans</h2>
                    {postnordOptions.length === 0 ? (
                    <p>Inga hemleveransalternativ tillgängliga.</p>
                    ) : (
                    postnordOptions.map((warehouse, wIndex) => (
                        <div key={wIndex} className="warehouse-options">
                        <h3 style={{textAlign:"left"}}>{warehouse.warehouse.address.city}</h3>
                        {warehouse.deliveryOptions.map((option, oIndex) => (
                            <div key={oIndex} className="delivery-option">
                            <input
                                type="radio"
                                name="deliveryOption"
                                value={option.defaultOption.bookingInstructions.deliveryOptionId}
                                onChange={() =>
                                handleDeliverySelect(option.defaultOption.bookingInstructions)
                                }
                            />
                            <label className="delivery-option-label">
                                <img src="./IMG/postnordIcon.jpg" alt="Postnord" style={{width:30,height:30}}/>
                                <div>
                                    <div style={{display:"flex",flexDirection:"row",alignItems:"center",gap:"5px",margin:0}}>
                                    <p>Postnord</p>
                                    {option.defaultOption.descriptiveTexts.checkout.title}
                                    </div>             
                                <p className="delivery-time" style={{justifySelf:"start",marginTop:0,lineHeight:1}}>{option.defaultOption.descriptiveTexts.checkout.friendlyDeliveryInfo}</p> 
                                </div> 
                                <p className="delivery-description">{option.defaultOption.descriptiveTexts.checkout.briefDescription}</p>   
                                <label style={{backgroundColor:"rgba(15, 23, 42, 0.95)",padding:10,borderRadius:10,color:"white"}}>
                                39 kr
                                </label>                           
                            </label>
                            
                            </div>
                        ))}
                        </div>
                    ))
                    )}
                </div>
            )}
      
            {clientSecret && (
                <div id="payment">
                <h2 style={{textAlign:"left"}}>Betalning</h2>
                 <Elements stripe={stripePromise} options={{clientSecret, appearance, loader}}>
                  <CheckoutForm></CheckoutForm>
                </Elements>
                </div>
            )}      
        
        </div>
    )
}