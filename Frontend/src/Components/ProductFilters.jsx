import {useState, useEffect} from 'react'
import {CategoryService} from '../Services/CategoryService'
import "../CSS/ProductFilter.css"

export default function ProductFilters({
    filters,
    updateFilter,
}){
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
    const loadCategories = async () => {
        try {
            const data = await CategoryService.getCategoryTree(); 
            setCategories(Array.isArray(data) ? data : data.categories);
        } catch (err) {
            console.error("Failed to load categories",err);
        }finally{
            setLoading(false);
        }
    };

    loadCategories();
}, []);

    return (
        <div className ="filter-container">
            <h3>Sortera:</h3>

            <select 
            value ={filters.brand} 
            onChange={(e) => updateFilter({brand: e.target.value})}>

                <option value="">Alla</option>
                <option value="Scan">Scan</option>
                <option value="ICA">ICA</option>
            </select>

            <div className="category-menu">
                <button className="menu-button">Välj kategori</button>
                

                <div className="menu-dropdown">
                    <div 
                        className="menu-item"
                        onClick={() => updateFilter({
                            categoryId: null,
                            subCategoryId: null,
                            subSubCategoryId: null
                        })}                       
                    >
                        Alla kategorier
                    </div>

                    {categories.map(cat => (
                        <div className="menu-item" key={cat.id}>
                            <span 
                                onClick={() => 
                                        updateFilter({
                                                    categoryId: cat.id,
                                                    subCategoryId: null,
                                                    subSubCategoryId: null
                                                })
                                }
                            >
                                {cat.mainCategory}
                                {cat.subCategories?.length > 0 && <span className="arrow">▶</span>}
                            </span>

                                {/*Sub category */}
                            <div className="submenu">
                                {cat.subCategories?.map(sc => (
                                    <div className="submenu-item" key={sc.id}>
                                        <span
                                            onClick={() =>                                              
                                                updateFilter({
                                                    categoryId: cat.id,
                                                    subCategoryId: sc.id,
                                                    subSubCategoryId: null
                                                })
                                            }
                                        >
                                            {sc.name}
                                            {sc.subSubCategories?.length > 0 && <span className="arrow">▶</span>}
                                        </span>

                                        {/* Subsub category */}
                                        <div className="subsubmenu">
                                            {sc.subSubCategories?.map(ss => (
                                                <div 
                                                    key={ss.id} 
                                                    className="subsubmenu-item"
                                                    onClick={() => 
                                                        updateFilter({
                                                            categoryId: cat.id,
                                                            subCategoryId: sc.id,
                                                            subSubCategoryId: ss.id
                                                        })
                                                    }
                                                >
                                                    {ss.name}                                                    
                                                </div>
                                            ))}
                                        </div>

                                    </div>
                                ))}
                            </div>

                        </div>
                    ))}
                </div>
            </div>

             <input  
                type="number"
                placeholder="Pris"
                value={filters.price}
                onChange={(e) => updateFilter({price: Number(e.target.value)})}
            />

            <button className="reset-filter" onClick = {() => updateFilter({
                categoryId: null,
                subCategoryId: null,
                subSubCategoryId: null,
                price: null,
                brand: null
            })}>Rensa filter</button>
            
        </div>
    )
}