import { Tabs } from "expo-router";
import Ionicons from '@expo/vector-icons/Ionicons';
import { useSafeAreaInsets } from "react-native-safe-area-context";

type TabRoutes = "index" | "explore";

interface TabConfig {
  name: TabRoutes;
  title: string;
  icon: any;
}

const TABS: TabConfig[] = [
  { name: "index", title: "Inicio", icon: "chatbubble-outline" },
  { name: "explore", title: "Explorar", icon: "chatbubble-outline" },
];

export default function TabLayout() {
  const insets = useSafeAreaInsets();
  return (
    <Tabs
      screenOptions={{
        headerShown: false,
        tabBarStyle: {
          height: 64 + insets.bottom,
          paddingBottom: insets.bottom,
        },
        tabBarItemStyle: { marginTop: 6 },
      }}
    >
      {TABS.map((tab) => (
        <Tabs.Screen
          key={tab.name}
          name={tab.name}
          options={{
            title: tab.title,
            tabBarIcon: ({ color, size }) => (
              <Ionicons name={tab.icon} size={size} color={color} />
            ),
          }}
        />
      ))}
    </Tabs>
  );
}
