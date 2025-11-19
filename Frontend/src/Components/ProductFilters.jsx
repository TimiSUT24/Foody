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
            <h3>Filters</h3>

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
                placeholder="Price"
                value={filters.price}
                onChange={(e) => updateFilter({price: Number(e.target.value)})}
            />
            
        </div>
    )
}