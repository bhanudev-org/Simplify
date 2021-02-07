import './App.css'
import { AiFillHome, AiOutlineInbox } from 'react-icons/ai'
import { BsFillCameraVideoFill } from 'react-icons/bs'
import { FaMoon, FaSun } from 'react-icons/fa'
import Logo from './assets/images/logo.png'
import { BrowserRouter as Router, Switch, Route, Link as RouterLink } from 'react-router-dom'
import { chakra, Flex, HStack, Link, Button, useColorModeValue, Box, useDisclosure, Spacer, IconButton, VStack, CloseButton, useColorMode } from '@chakra-ui/react'
import { useContext, useEffect } from 'react'
import { GlobalContext } from './utils/GlobalContext'

const Home = () => {
  const context = useContext(GlobalContext)
  useEffect(() => {
    context.handleLoading(false)
  }, [])
  return <h2>Home - {console.log(context)}</h2>
}

function About() {
  return <h2>About</h2>
}

function Users() {
  return <h2>Users</h2>
}

function App() {
  const bg = useColorModeValue('white', 'gray.800')
  const cl = useColorModeValue('gray.800', 'white')
  const mobileNav = useDisclosure()
  const { toggleColorMode: toggleMode } = useColorMode()
  const text = useColorModeValue('dark', 'light')
  const SwitchIcon = useColorModeValue(FaMoon, FaSun)

  const MobileNavContent = (
    <VStack pos="absolute" top={0} left={0} right={0} display={mobileNav.isOpen ? 'flex' : 'none'} flexDirection="column" p={2} pb={4} m={2} bg={bg} spacing={3} rounded="sm" boxShadow="sm">
      <CloseButton aria-label="Close menu" justifySelf="self-start" onClick={mobileNav.onClose} />
      <Button w="100%" variant="ghost" href="#" leftIcon={<AiFillHome />}>
        Dashboard
      </Button>
      <Button w="100%" variant="solid" colorScheme="brand" href="#" leftIcon={<AiOutlineInbox />}>
        Inbox
      </Button>
      <Button w="100%" variant="ghost" href="#" leftIcon={<BsFillCameraVideoFill />}>
        Videos
      </Button>
    </VStack>
  )

  return (
    <>
      <Router>
        <chakra.header h="100%" bg={bg} w="100%" px={{ base: 2, sm: 4 }} py={4}>
          <Flex alignItems="center" justifyContent="space-between" mx="auto">
            <Link display="flex" alignItems="center" href="/">
              <img src={Logo} alt="logo" width={100} height={100} />
            </Link>
            <Box display={{ base: 'none', md: 'inline-flex' }}>
              <HStack spacing={1}>
                <Button to="/" as={RouterLink} bg={bg} color="gray.500" alignItems="center" fontSize="md" _hover={{ color: cl }} _focus={{ boxShadow: 'none' }}>
                  Home
                </Button>
                <Button to="/users" as={RouterLink} bg={bg} color="gray.500" display="inline-flex" alignItems="center" fontSize="md" _hover={{ color: cl }} _focus={{ boxShadow: 'none' }}>
                  Users
                </Button>
                <Button to="/about" as={RouterLink} bg={bg} color="gray.500" display="inline-flex" alignItems="center" fontSize="md" _hover={{ color: cl }} _focus={{ boxShadow: 'none' }}>
                  About
                </Button>
              </HStack>
            </Box>
            <Spacer />
            <Box display="flex" alignItems="center">
              <HStack spacing={1}>
                <Button to="/sign-in" as={RouterLink} colorScheme="brand" variant="ghost" size="sm">
                  Sign in
                </Button>
                <Button to="/sign-up" as={RouterLink} colorScheme="brand" variant="solid" size="sm">
                  Sign up
                </Button>
              </HStack>
              <IconButton size="md" fontSize="lg" aria-label={`Switch to ${text} mode`} variant="ghost" color="current" ml={{ base: '0', md: '3' }} onClick={toggleMode} icon={<SwitchIcon />} />
            </Box>
          </Flex>

          {MobileNavContent}
        </chakra.header>

        <Box width={{ base: '100%', md: 11 / 12, xl: 8 / 12 }} textAlign={{ base: 'left', md: 'center' }} mx="auto">
          {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}
          <Switch>
            <Route path="/about">
              <About />
            </Route>
            <Route path="/users">
              <Users />
            </Route>
            <Route path="/">
              <Home />
            </Route>
          </Switch>
        </Box>
      </Router>
    </>
  )
}

export default App
