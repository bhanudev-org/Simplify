import React, {useContext,createContext,useReducer} from 'react'

export const GlobalContext = createContext({})

// export const useGlobalContext = useContext(GlobalContext)

const initialState = {
    authenticated: true,
    user: null,
    loading: false,
    handleLoading: (isLoading) => {},
    handleAuthenticated: (isAuthenticated) => {},
    handleUserInfo: (userInfo) => {},
}

const reducer = (state, action) => {
    switch (action.type) {
        case 'loading':
            return {
                ...state,
                loading: action.payload ?? false,
            }
        case 'authenticated':
            return {
                ...state,
                authenticated: action.payload ?? false,
            }
        case 'user':
            return {
                ...state,
                user: action.payload,
            }

        default:
            return state
    }
}

export const GlobalContextProvider = ({children})=>{
    const [state, dispatch] = useReducer(reducer, initialState)

    return(
        <GlobalContext.Provider value={{...state,
            handleLoading: (isLoading) => dispatch({ type: 'loading', payload: isLoading }),
            handleAuthenticated: (isAuthenticated) => dispatch({ type: 'authenticated', payload: isAuthenticated }),
                handleUserInfo: (userInfo) => dispatch({ type: 'user', payload: userInfo }),
        }}>
            {children}
        </GlobalContext.Provider>
    )
}